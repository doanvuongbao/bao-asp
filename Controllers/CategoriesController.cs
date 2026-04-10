using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using bao_asp.Data;
using bao_asp.Models;

namespace bao_asp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriesController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Categories>>> GetCategories()
        {
            return await _context.Categories.ToListAsync();
        }

        [HttpPost]
        public async Task<ActionResult<Categories>> PostCategory([FromBody] Categories category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return Ok(category);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}