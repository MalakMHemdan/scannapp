using System.ComponentModel.DataAnnotations;

public class Points
{
    [Key]
    public int PointsID { get; set; }
    public int UserId { get; set; }
    public int BranchId { get; set; }
    public int AmountOfPoints { get; set; }
    public int AdminID { get; set; }  
}
