// Product Dto to return specefic data
public class ProductDto
{
    public Guid ProductId { get; set; }
    public List<SizeDto> Sizes { get; set; }
    public string Title { get; set; }
    public List<ColorDto> Colors { get; set; }
    public Material Material { get; set; } 
    public string Image { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}