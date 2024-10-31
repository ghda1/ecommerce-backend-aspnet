// public list for t-shit size
using System.ComponentModel.DataAnnotations;

public class Size
{
    public Guid SizeId { get; set; }

    [Required(ErrorMessage = "Size value is missing.")]
    public string Value { get; set; } = string.Empty;

    public List<Product> Products { get; set; }


}

