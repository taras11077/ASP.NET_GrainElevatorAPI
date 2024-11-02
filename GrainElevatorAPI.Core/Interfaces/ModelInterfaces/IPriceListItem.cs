namespace GrainElevatorAPI.Core.Interfaces.ModelInterfaces;

public interface IPriceListItem
{
    int Id { get; set; }
    int TechnologicalOperationId { get; set; }
    decimal  OperationPrice { get; set; }
    int PriceListId { get; set; }
}