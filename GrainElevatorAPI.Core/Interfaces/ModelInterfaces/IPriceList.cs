using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Interfaces.ModelInterfaces;

public interface IPriceList
{
    int Id { get; set; }
    int ProductId { get; set; }
    
    bool? IsFinalized { get; set; }
    
    ICollection<CompletionReport> CompletionReports { get; set; }
    ICollection<PriceListItem> PriceListItems { get; set; }
}