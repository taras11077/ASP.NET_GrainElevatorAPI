using System.ComponentModel.DataAnnotations;
using GrainElevatorAPI.Core.Interfaces.ModelInterfaces;
using GrainElevatorAPI.Core.Models.Base;

namespace GrainElevatorAPI.Core.Models;

public class PriceList : AuditableEntity, IPriceList
{
	[Required(ErrorMessage = "Id is required.")]
	[Range(1, int.MaxValue, ErrorMessage = "ProductWeight must be a positive number.")]
	public int Id { get; set; }

	[Required(ErrorMessage = "Id is required.")]
	[Range(1, int.MaxValue, ErrorMessage = "ProductWeight must be a positive number.")]
	public int ProductId { get; set; }
	
	public virtual Product Product { get; set; }
	
    public virtual ICollection<CompletionReport> CompletionReports { get; set; } = new List<CompletionReport>();
    public virtual ICollection<PriceListItem> PriceListItems { get; set; } = new List<PriceListItem>();

}

