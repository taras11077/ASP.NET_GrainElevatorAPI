namespace GrainElevatorAPI.Core.Models;

public class PriceByOperation
{
    public int Id { get; set; }
    public string OperationTitle { get; set; }
    public double OperationPrice { get; set; }
    public int PriceListId { get; set; }

    public virtual PriceList PriceList { get; set; }

}

