namespace GrainElevatorAPI.Core.Interfaces.ModelInterfaces;

public interface IWarehouseProductCategory
{
    int Id { get; set; }
    string Title { get; set; }
    int? Value { get; set; }
    int? WarehouseUnitId { get; set; }
}