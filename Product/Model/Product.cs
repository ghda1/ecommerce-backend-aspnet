// Product Model
using System.Text.Json.Serialization;

public class Product
{
    public Guid ProductId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Image { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public Material Material { get; set; }
    public int Quantity { get; set; }

    // one Product has a list of Size 
    [JsonIgnore]
    public List<Size> Sizes { get; set; }

    // one Product has a list of Color 
    [JsonIgnore]
    public List<Color> Colors { get; set; }

    // one Product has one orderDetails => one product could be in many orderDetail
    [JsonIgnore]
    public List<OrderDetails> OrderDetailses { get; set; }
}