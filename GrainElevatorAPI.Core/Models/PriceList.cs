using System.ComponentModel.DataAnnotations;

namespace GrainElevatorAPI.Core.Models;

public class PriceList
{
	[Required(ErrorMessage = "Id is required.")]
	[Range(1, int.MaxValue, ErrorMessage = "ProductWeight must be a positive number.")]
	public int Id { get; set; }

	[Required(ErrorMessage = "ProductTitle is required.")]
	[MinLength(4, ErrorMessage = "ProductTitle must be at least 4 characters long.")]
	[MaxLength(20, ErrorMessage = "ProductTitle must be at least 20 characters long.")]
	public string ProductTitle { get; set; }

	
	
	
	[Range(1, int.MaxValue, ErrorMessage = "CreatedById must be a positive number.")]
	public int CreatedById { get; set; }
	[Range(1, int.MaxValue, ErrorMessage = "ModifiedById must be a positive number.")]
	public int? ModifiedById { get; set; }
	[Range(1, int.MaxValue, ErrorMessage = "RemovedById must be a positive number.")]
	public int? RemovedById { get; set; }
	[Range(1, int.MaxValue, ErrorMessage = "RestoreById must be a positive number.")]
	public int? RestoreById { get; set; }

    public virtual ICollection<CompletionReport> CompletionReports { get; set; } = new List<CompletionReport>();
    public virtual ICollection<PriceListItem> PriceByOperations { get; set; } = new List<PriceListItem>();
    public virtual Employee? CreatedBy { get; set; }
    public virtual Employee? ModifiedBy { get; set; }
    public virtual Employee? RemovedBy { get; set; }
    public virtual Employee? RestoreBy { get; set; }

}

