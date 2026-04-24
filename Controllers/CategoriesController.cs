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

        // --- PHẦN THÊM MỚI: Cập nhật loại sách ---
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCategory(int id, [FromBody] Categories category)
        {
            // Kiểm tra ID trên URL và ID trong dữ liệu gửi lên có khớp nhau không
            if (id != category.Id)
            {
                return BadRequest("ID không khớp"); // Trả về lỗi 400 nếu không khớp
            }

            // Đánh dấu thực thể đã bị thay đổi để EF Core cập nhật
            _context.Entry(category).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // Xử lý lỗi nếu bản ghi đã bị xóa trong lúc đang cập nhật
                if (!CategoryExists(id))
                {
                    return NotFound("Không tìm thấy loại sách để cập nhật");
                }
                else
                {
                    throw;
                }
            }

            return Ok(category); // Trả về dữ liệu đã cập nhật thành công
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

        // Hàm hỗ trợ kiểm tra sự tồn tại của Category
        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }
}