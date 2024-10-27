using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Interfaces.ModelInterfaces;

public interface IProductionPriceList
{
    int Id { get; set; }

    string ProductTitle { get; set; }
    
    ICollection<CompletionReport> CompletionReports { get; set; }
    ICollection<PriceListItem> PriceListItems { get; set; }
}