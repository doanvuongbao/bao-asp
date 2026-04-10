using System.Text.Json.Serialization;
public class OrderItem
{
    public int OrderId { get; set; }
    public int BookId { get; set; }
    public int Quantity { get; set; }

    // Thêm đơn giá để lưu lại giá tại thời điểm mua
    public decimal UnitPrice { get; set; }

    // QUAN TRỌNG: Thêm dấu ? để báo cho Swagger rằng 
    // không cần gửi kèm nguyên cả đối tượng Order/Book lên.
    [JsonIgnore]
    public virtual Order? Order { get; set; }
    [JsonIgnore]
    public virtual Book? Book { get; set; }
}