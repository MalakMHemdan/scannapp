using System.ComponentModel.DataAnnotations;

public class Order
{
    [Key]
    public int OrderId { get; set; }
    public int UserId { get; set; }
    public int BranchId { get; set; }  
    public decimal SubTotal { get; set; }
    public decimal Discount_Amount { get; set; }
    public decimal Total_Amount { get; set; }
    public bool BonusApplied { get; set; }  
    public DateTime CreatedAt { get; set; }
}