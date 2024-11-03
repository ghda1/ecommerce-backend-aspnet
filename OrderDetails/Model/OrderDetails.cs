//Order Details Model
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

public class OrderDetails
{
    public Guid OrdersDetailesId { get; set; }
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }

    // one order has many order detailses 
    public Guid OrderId { get; set; }

    [JsonIgnore]
    public Order Order { get; set; }

    // one product has many order detailses 
    public Guid ProductId { get; set; }

    [JsonIgnore]
    public Product Product { get; set; }

    // one orderDetaile has one size 
    public Guid SizeId { get; set; }

    [JsonIgnore]
    public Size Size { get; set; }

    // one orderDetaile has one color 
    public Guid ColorId { get; set; }

    [JsonIgnore]
    public Color Color { get; set; }

}