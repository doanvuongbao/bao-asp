using bao_asp.Data;
using bao_asp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace bao_asp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public OrdersController(AppDbContext context)
        {
            _context = context;
        }

        // 1. LẤY TẤT CẢ ĐƠN HÀNG
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                .ToListAsync();
        }

        // 2. LẤY CHI TIẾT 1 ĐƠN HÀNG
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null) return NotFound();
            return order;
        }

        // 3. HÀM CHECKOUT (Sửa lỗi gán sai biến Orders -> OrderDate)
        [HttpPost("checkout")]
        public async Task<IActionResult> Checkout([FromBody] List<OrderItemRequest> cartItems, [FromQuery] int userId = 1)
        {
            if (cartItems == null || !cartItems.Any())
                return BadRequest("Giỏ hàng trống.");

            var newOrder = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now, // ĐÃ SỬA: Trước đó bạn ghi là 'Orders = ...' dẫn đến lỗi
                Status = "Pending",
                OrderItems = new List<OrderItem>()
            };

            decimal total = 0;

            foreach (var item in cartItems)
            {
                var book = await _context.Books.FindAsync(item.BookId);
                if (book == null) continue;

                var orderDetail = new OrderItem
                {
                    BookId = item.BookId,
                    Quantity = item.Quantity,
                    UnitPrice = book.Price
                };

                total += orderDetail.Quantity * orderDetail.UnitPrice;
                newOrder.OrderItems.Add(orderDetail);
            }

            newOrder.TotalPrice = total;

            _context.Orders.Add(newOrder);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Thanh toán thành công!",
                orderId = newOrder.Id,
                totalAmount = newOrder.TotalPrice
            });
        }

        // 4. CẬP NHẬT TRẠNG THÁI
        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] OrderStatusUpdate request)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return NotFound();

            order.Status = request.Status;
            await _context.SaveChangesAsync();

            return Ok(order);
        }

        // 5. XÓA ĐƠN HÀNG
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null) return NotFound();

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // Classes hỗ trợ
        public class OrderItemRequest
        {
            public int BookId { get; set; }
            public int Quantity { get; set; }
        }

        public class OrderStatusUpdate
        {
            public string Status { get; set; }
        }
    }
}