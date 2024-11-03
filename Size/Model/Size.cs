// public list for t-shit size
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class Size
{
    public Guid SizeId { get; set; }

    [Required(ErrorMessage = "Size value is missing.")]
    public string Value { get; set; } = string.Empty;

    [JsonIgnore]
    public List<Product> Products { get; set; }

}

