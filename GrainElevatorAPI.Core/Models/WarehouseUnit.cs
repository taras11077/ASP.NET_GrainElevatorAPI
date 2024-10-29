using System.ComponentModel.DataAnnotations;
using GrainElevatorAPI.Core.Interfaces.ModelInterfaces;
using GrainElevatorAPI.Core.Models.Base;

namespace GrainElevatorAPI.Core.Models;

public class WarehouseUnit : AuditableEntity, IWarehouseUnit
{
	[Required(ErrorMessage = "Id is required.")]
	[Range(1, int.MaxValue, ErrorMessage = "Id must be a positive number.")]
	public int Id { get; set; }
	
	public virtual ICollection<WarehouseProductCategory> ProductCategories { get; set; } = new List<WarehouseProductCategory>();
	public virtual ICollection<OutputInvoice> OutputInvoices { get; set; } = new List<OutputInvoice>();
	
	[Range(1, int.MaxValue, ErrorMessage = "SupplierId must be a positive number.")]
	public int SupplierId { get; set; }
	
	[Range(1, int.MaxValue, ErrorMessage = "ProductId must be a positive number.")]
	public int ProductId { get; set; }
	
	public virtual Supplier Supplier { get; set; }
	public virtual Product Product { get; set; }
}
