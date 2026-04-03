using bao_asp.Models;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/cart")]
public class CartController : ControllerBase
{
    private static List<OrderItem> cart = new List<OrderItem>();

    // GET: api/cart
    [HttpGet]
    public IActionResult GetCart()
    {
        return Ok(cart);
    }

    // GET: api/cart/1
    [HttpGet("{bookId}")]
    public IActionResult GetCartItem(int bookId)
    {
        var item = cart.FirstOrDefault(x => x.BookId == bookId);

        if (item == null)
        {
            return NotFound();
        }

        return Ok(item);
    }

    // POST: api/cart
    [HttpPost]
    public IActionResult AddToCart([FromBody] OrderItem item)
    {
        var existingItem = cart.FirstOrDefault(x => x.BookId == item.BookId);

        if (existingItem != null)
        {
            existingItem.Quantity += item.Quantity;
        }
        else
        {
            cart.Add(item);
        }

        return Ok(cart);
    }

    // PUT: api/cart/1
    [HttpPut("{bookId}")]
    public IActionResult UpdateCart(int bookId, [FromBody] OrderItem updatedItem)
    {
        var item = cart.FirstOrDefault(x => x.BookId == bookId);

        if (item == null)
        {
            return NotFound();
        }

        item.Quantity = updatedItem.Quantity;
        item.UnitPrice = updatedItem.UnitPrice;

        return Ok(item);
    }

    // DELETE: api/cart/1
    [HttpDelete("{bookId}")]
    public IActionResult RemoveFromCart(int bookId)
    {
        var item = cart.FirstOrDefault(x => x.BookId == bookId);

        if (item == null)
        {
            return NotFound();
        }

        cart.Remove(item);

        return Ok(cart);
    }

    // DELETE: api/cart
    [HttpDelete]
    public IActionResult ClearCart()
    {
        cart.Clear();
        return Ok("Đã xóa toàn bộ giỏ hàng");
    }
}