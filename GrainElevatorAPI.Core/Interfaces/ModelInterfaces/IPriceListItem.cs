namespace GrainElevatorAPI.Core.Interfaces.ModelInterfaces;

public interface IPriceListItem
{
    int Id { get; set; }
    string OperationTitle { get; set; }
    double OperationPrice { get; set; }
    int PriceListId { get; set; }

}