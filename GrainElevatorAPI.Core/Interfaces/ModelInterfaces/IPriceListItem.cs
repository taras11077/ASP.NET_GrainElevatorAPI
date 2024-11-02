namespace GrainElevatorAPI.Core.Interfaces.ModelInterfaces;

public interface IPriceListItem
{
    int Id { get; set; }
    string OperationName { get; set; }
    decimal  OperationPrice { get; set; }
    int PriceListId { get; set; }
}