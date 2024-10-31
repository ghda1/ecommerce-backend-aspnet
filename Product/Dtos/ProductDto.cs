// Product Dto to return specefic data
public class ProductDto
{
    public List<Size> Size { get; set; }
    public string Title { get; set; }
    public List<Color> Color { get; set; }
    public Material Material { get; set; }
    public string Image { get; set; }
    public decimal Price { get; set; }
    public int Quantity { get; set; }
}