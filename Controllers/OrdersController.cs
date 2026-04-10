using bao_asp.Data;
using bao_asp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly AppDbContext _context;

    public OrdersController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/orders
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
    {
        return await _context.Orders
            .Include(o => o.OrderItems)
            .ToListAsync();
    }

    // GET: api/orders/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Order>> GetOrder(int id)
    {
        var order = await _context.Orders
            .Include(o => o.OrderItems)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
        {
            return NotFound();
        }

        return order;
    }

    // GET: api/orders/user/1
    [HttpGet("user/{userId}")]
    public async Task<ActionResult<IEnumerable<Order>>> GetOrdersByUser(int userId)
    {
        return await _context.Orders
            .Where(o => o.UserId == userId)
            .Include(o => o.OrderItems)
            .ToListAsync();
    }

    
    // POST: api/orders
    [HttpPost]
    public async Task<ActionResult<Order>> PostOrder([FromBody] Order order)
    {
        foreach (var item in order.OrderItems)
        {
            var book = await _context.Books.FindAsync(item.BookId);

            if (book == null)
            {
                return BadRequest($"Không tìm thấy sách có Id = {item.BookId}");
            }

            item.UnitPrice = book.Price;
        }

        order.TotalPrice = order.OrderItems.Sum(x => x.Quantity * x.UnitPrice);

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        return Ok(order);
    }

    // PUT: api/orders/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateOrder(int id, [FromBody] Order updatedOrder)
    {
        if (id != updatedOrder.Id)
        {
            return BadRequest();
        }

        var order = await _context.Orders.FindAsync(id);

        if (order == null)
        {
            return NotFound();
        }

        order.UserId = updatedOrder.UserId;
        order.TotalPrice = updatedOrder.TotalPrice;
        order.Status = updatedOrder.Status;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // PUT: api/orders/{id}/status
    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] Order updatedOrder)
    {
        var order = await _context.Orders.FindAsync(id);

        if (order == null)
        {
            return NotFound();
        }

        order.Status = updatedOrder.Status;
        await _context.SaveChangesAsync();

        return Ok(order);
    }

    // DELETE: api/orders/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteOrder(int id)
    {
        var order = await _context.Orders.FindAsync(id);

        if (order == null)
        {
            return NotFound();
        }

        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}