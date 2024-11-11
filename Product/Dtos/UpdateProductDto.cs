// Update Product Dto
using System.ComponentModel.DataAnnotations;
public class UpdateProductDto
{
    public string? Image { get; set; }
    public string? Title { get; set; }
    public List<Guid> SizeIds { get; set; }
    public List<Guid> ColorIds { get; set; }
    public Material? Material { get; set; }
    public int? Quantity { get; set; }
    public decimal? Price { get; set; }
}