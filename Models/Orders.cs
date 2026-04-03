using System.ComponentModel.DataAnnotations.Schema;

public class Order
{
    public int Id { get; set; }
    public int UserId { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalPrice { get; set; }

    public string Status { get; set; } = "Pending"; // Gán giá trị mặc định là 'Chờ xử lý'  

    // Thêm dấu ? để không bắt buộc phải gửi nguyên object User lên từ Swagger
    public virtual User? User { get; set; }

    // Thêm dấu ? và khởi tạo danh sách rỗng để tránh lỗi NullReference
    public virtual List<OrderItem>? OrderItems { get; set; } = new List<OrderItem>();
}