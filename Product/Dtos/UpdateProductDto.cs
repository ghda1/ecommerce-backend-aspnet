// Update Product Dto
using System.ComponentModel.DataAnnotations;
public class UpdateProductDto
{
    public string? Image { get; set; } = string.Empty;
    public string? Title { get; set; } = string.Empty;
    public List<Guid> SizeIds { get; set; } = new List<Guid>();
    public List<Guid> ColorIds { get; set; } = new List<Guid>();
    public Material? Material { get; set; }
    public int? Quantity { get; set; }
    public decimal? Price { get; set; }
}