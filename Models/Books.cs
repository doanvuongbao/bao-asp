using System.ComponentModel.DataAnnotations.Schema;

public class Book
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Author { get; set; }
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }
    public int Stock { get; set; }
    public int CategoryId { get; set; }

    // Thêm dấu ? và khởi tạo List trống mặc định
    public List<OrderItem>? OrderItems { get; set; } = new List<OrderItem>();
}