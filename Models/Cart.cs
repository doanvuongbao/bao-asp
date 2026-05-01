namespace bao_asp.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int BookId { get; set; }
        public int Quantity { get; set; }

        // Nếu bạn có quan hệ với các bảng khác
        public User? User { get; set; }
        public Book? Book { get; set; }
    }
}