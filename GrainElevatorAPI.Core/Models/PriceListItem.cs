using System.ComponentModel.DataAnnotations;
using GrainElevatorAPI.Core.Interfaces.ModelInterfaces;
using GrainElevatorAPI.Core.Models.Base;

namespace GrainElevatorAPI.Core.Models;

public class PriceListItem : AuditableEntity, IPriceListItem
{
	[Required(ErrorMessage = "Id is required.")]
	[Range(1, int.MaxValue, ErrorMessage = "Id must be a positive number.")]
	public int Id { get; set; }

	[Required(ErrorMessage = "OperationTitle is required.")]
	[MinLength(4, ErrorMessage = "OperationTitle must be at least 4 characters long.")]
	[MaxLength(20, ErrorMessage = "OperationTitle must be at least 20 characters long.")]
	public string OperationTitle { get; set; }

	[Range(0, double.MaxValue, ErrorMessage = "ProductWeight must be a positive number.")]
	public double OperationPrice { get; set; }
	

	[Range(1, int.MaxValue, ErrorMessage = "ProductWeight must be a positive number.")]
	public int PriceListId { get; set; }

    public virtual ProductionPriceList ProductionPriceList { get; set; }
}

