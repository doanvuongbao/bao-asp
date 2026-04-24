using bao_asp.Models;
using bao_asp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/cart")]
public class CartController : ControllerBase
{
    private readonly AppDbContext _context;

    // Danh sách tĩnh để lưu tạm giỏ hàng (Sẽ mất dữ liệu khi restart server)
    private static List<OrderItem> cart = new List<OrderItem>();

    public CartController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/cart
    // Trả về danh sách OrderItem kèm theo Tiêu đề sách lấy từ DB
    [HttpGet]
    public IActionResult GetCart()
    {
        var cartWithDetails = cart.Select(item => {
            // Tìm sách trong database để lấy Title
            var book = _context.Books.FirstOrDefault(b => b.Id == item.BookId);
            return new
            {
                item.BookId,
                item.Quantity,
                item.UnitPrice,
                // Chỉ lấy Title, không lấy Image
                Title = book?.Title ?? "N/A",
                Total = item.Quantity * item.UnitPrice
            };
        }).ToList();

        return Ok(cartWithDetails);
    }

    // POST: api/cart
    [HttpPost]
    public IActionResult AddToCart([FromBody] OrderItem item)
    {
        // Kiểm tra sách có tồn tại trong Database không
        var book = _context.Books.FirstOrDefault(x => x.Id == item.BookId);
        if (book == null) return NotFound("Không tìm thấy sách");

        // Cập nhật giá bán mới nhất từ sách
        item.UnitPrice = book.Price;

        var existingItem = cart.FirstOrDefault(x => x.BookId == item.BookId);
        if (existingItem != null)
        {
            // Nếu đã có trong giỏ thì cộng dồn số lượng
            existingItem.Quantity += item.Quantity;
            existingItem.UnitPrice = book.Price;
        }
        else
        {
            // Nếu chưa có thì thêm mới vào list tĩnh
            cart.Add(item);
        }

        return Ok(cart);
    }

    // DELETE: api/cart/{bookId}
    [HttpDelete("{bookId}")]
    public IActionResult RemoveFromCart(int bookId)
    {
        var item = cart.FirstOrDefault(x => x.BookId == bookId);
        if (item == null) return NotFound();

        cart.Remove(item);
        return Ok(new { message = "Đã xóa sản phẩm khỏi giỏ hàng", cart });
    }

    // DELETE: api/cart
    [HttpDelete]
    public IActionResult ClearCart()
    {
        cart.Clear();
        return Ok(new { message = "Đã xóa toàn bộ giỏ hàng thành công" });
    }
}