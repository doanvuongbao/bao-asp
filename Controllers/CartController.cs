using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/cart")]
public class CartController : ControllerBase
{
    private static List<OrderItem> cart = new List<OrderItem>();

    [HttpPost]
    public IActionResult AddToCart(OrderItem item)
    {
        cart.Add(item);
        return Ok(cart);
    }

    [HttpGet]
    public IActionResult GetCart()
    {
        return Ok(cart);
    }
}