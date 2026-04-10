using System.ComponentModel.DataAnnotations.Schema;

public class Order
{
    public int Id { get; set; }
    public int UserId { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal TotalPrice { get; set; }

    public string Status { get; set; } = "Pending";

    public virtual List<OrderItem>? OrderItems { get; set; } = new List<OrderItem>();
}