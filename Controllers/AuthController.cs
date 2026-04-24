using bao_asp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    public AuthController(AppDbContext context) => _context = context;

    // POST: api/auth/register
    [HttpPost("register")]
    public async Task<IActionResult> Register(User user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return Ok(new { message = "Đăng ký thành công!" });
    }

    // POST: api/auth/login
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] User loginInfo)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == loginInfo.Email && u.Password == loginInfo.Password);

        if (user == null) return Unauthorized("Sai email hoặc mật khẩu");
        return Ok(new { message = "Đăng nhập thành công", role = user.Role });
    }
    // Thêm hàm này vào trong class AuthController
    [HttpGet("getall")]
    public async Task<IActionResult> GetAll()
    {
        // Chỉ lấy Id, Name, Email và Role để hiển thị lên bảng quản lý
        var users = await _context.Users
            .Select(u => new { u.Id, u.Name, u.Email, u.Role })
            .ToListAsync();
        return Ok(users);
    }
    // 3. CẬP NHẬT: PUT api/auth/update
    [HttpPut("update")]
    public async Task<IActionResult> Update(User user)
    {
        var existingUser = await _context.Users.FindAsync(user.Id);
        if (existingUser == null) return NotFound("Không tìm thấy người dùng");

        existingUser.Name = user.Name;
        existingUser.Email = user.Email;
        existingUser.Role = user.Role;

        // Chỉ cập nhật mật khẩu nếu người dùng nhập mới
        if (!string.IsNullOrEmpty(user.Password))
        {
            existingUser.Password = user.Password;
        }

        await _context.SaveChangesAsync();
        return Ok(existingUser);
    }

    // 4. XÓA: DELETE api/auth/delete/{id}
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return NotFound("Không tìm thấy người dùng");

        _context.Users.Remove(user);
        await _context.SaveChangesAsync();
        return Ok(new { message = "Xóa thành công" });
    }
}