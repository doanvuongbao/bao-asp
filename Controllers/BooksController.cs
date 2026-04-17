using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using bao_asp.Data;
using bao_asp.Models;

[Route("api/[controller]")]
[ApiController]
public class BooksController : ControllerBase
{
    private readonly AppDbContext _context;

    public BooksController(AppDbContext context)
    {
        _context = context;
    }

    // GET: api/books
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
    {
        return await _context.Books.ToListAsync();
    }

    // GET: api/books/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<Book>> GetBook(int id)
    {
        var book = await _context.Books.FindAsync(id);

        if (book == null)
        {
            return NotFound();
        }

        return book;
    }

    // GET: api/books/search?keyword=toan
    [HttpGet("search")]
    public async Task<ActionResult<IEnumerable<Book>>> SearchBooks(string keyword)
    {
        return await _context.Books
            .Where(b => b.Title.Contains(keyword))
            .ToListAsync();
    }

    // GET: api/books/category/1
    [HttpGet("category/{categoryId}")]
    public async Task<ActionResult<IEnumerable<Book>>> GetBooksByCategory(int categoryId)
    {
        return await _context.Books
            .Where(b => b.CategoryId == categoryId)
            .ToListAsync();
    }

    // GET: api/books/author/Nguyen Van A
    [HttpGet("author/{author}")]
    public async Task<ActionResult<IEnumerable<Book>>> GetBooksByAuthor(string author)
    {
        return await _context.Books
            .Where(b => b.Author.Contains(author))
            .ToListAsync();
    }

    // GET: api/books/filter?minPrice=100000&maxPrice=300000
    [HttpGet("filter")]
    public async Task<ActionResult<IEnumerable<Book>>> FilterBooks(decimal minPrice, decimal maxPrice)
    {
        return await _context.Books
            .Where(b => b.Price >= minPrice && b.Price <= maxPrice)
            .ToListAsync();
    }

    // POST: api/books
    [HttpPost]
    public async Task<ActionResult<Book>> PostBook([FromBody] Book book)
    {
        // Kiểm tra xem CategoryId có tồn tại trong database không (nếu cần)
        var categoryExists = await _context.Categories.AnyAsync(c => c.Id == book.CategoryId);
        if (!categoryExists)
        {
            return BadRequest($"CategoryId = {book.CategoryId} không tồn tại.");
        }

        // Thêm sách mới vào DbContext
        _context.Books.Add(book);

        // Lưu thay đổi vào Database
        await _context.SaveChangesAsync();

        // Trả về kết quả (thông thường trả về 201 Created và kèm thông tin sách vừa tạo)
        return CreatedAtAction(nameof(GetBook), new { id = book.Id }, book);
    }

 

    // PUT: api/books/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> PutBook(int id, [FromBody] Book book)
    {
        if (id != book.Id)
        {
            return BadRequest();
        }

        var existingBook = await _context.Books.FindAsync(id);

        if (existingBook == null)
        {
            return NotFound();
        }

        existingBook.Title = book.Title;
        existingBook.Author = book.Author;
        existingBook.Price = book.Price;
        existingBook.Stock = book.Stock;
        existingBook.CategoryId = book.CategoryId;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    // DELETE: api/books/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBook(int id)
    {
        var book = await _context.Books.FindAsync(id);

        if (book == null)
        {
            return NotFound();
        }

        _context.Books.Remove(book);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}