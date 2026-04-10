using System.Text.Json.Serialization;

public class User
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Email { get; set; }

    public string Password { get; set; }

    public string Role { get; set; }

    [JsonIgnore]
    public List<Order> Orders { get; set; } = new();
}