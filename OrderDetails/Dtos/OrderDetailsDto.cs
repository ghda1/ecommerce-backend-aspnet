// Order Dto to return specific date
public class OrderDetailsDto
{
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }

    // one product has many order details
    //public Guid ProductId { get; set; }
    public ProductDto Product { get; set; }
    public SizeDto Size { get; set; }
    public ColorDto Color { get; set; }

}