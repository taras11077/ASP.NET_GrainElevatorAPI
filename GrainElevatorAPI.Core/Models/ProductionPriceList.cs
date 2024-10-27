using System.ComponentModel.DataAnnotations;
using GrainElevatorAPI.Core.Interfaces.ModelInterfaces;
using GrainElevatorAPI.Core.Models.Base;

namespace GrainElevatorAPI.Core.Models;

public class ProductionPriceList : AuditableEntity, IProductionPriceList
{
	[Required(ErrorMessage = "Id is required.")]
	[Range(1, int.MaxValue, ErrorMessage = "ProductWeight must be a positive number.")]
	public int Id { get; set; }

	[Required(ErrorMessage = "ProductTitle is required.")]
	[MinLength(4, ErrorMessage = "ProductTitle must be at least 4 characters long.")]
	[MaxLength(20, ErrorMessage = "ProductTitle must be at least 20 characters long.")]
	public string ProductTitle { get; set; }
	
	
    public virtual ICollection<CompletionReport> CompletionReports { get; set; } = new List<CompletionReport>();
    public virtual ICollection<PriceListItem> PriceListItems { get; set; } = new List<PriceListItem>();

}

