using System.ComponentModel.DataAnnotations;

namespace GrainElevatorAPI.Core.Models;

public class LaboratoryCard
{
	[Required(ErrorMessage = "Id is required.")]
	[Range(1, int.MaxValue, ErrorMessage = "Id must be a positive number.")]
	public int Id { get; set; }

	[MinLength(3, ErrorMessage = "LaboratoryCard Number must be at least 3 characters long.")]
	[MaxLength(9, ErrorMessage = "LaboratoryCard Number must be at least 9 characters long.")]
	public string LabCardNumber { get; set; }

	[Required(ErrorMessage = "WeedImpurity is required.")]
	[Range(0.0, 100.0, ErrorMessage = "WeedImpurity must be between 0.0 and 100.0")]
	public double WeedImpurity { get; set; }

	[Required(ErrorMessage = "Moisture is required.")]
	[Range(0.0, 100.0, ErrorMessage = "Moisture must be between 0.0 and 100.0")]
	public double Moisture { get; set; }

	[Range(0.0, 100.0, ErrorMessage = "GrainImpurity must be between 0.0 and 100.0")]
	public double? GrainImpurity { get; set; }

	[MaxLength(300, ErrorMessage = "OperationTitle must be at least 300 characters long.")]
	public string? SpecialNotes { get; set; }


    public bool? IsProduction { get; set; }
    
    
    [Required(ErrorMessage = "InputInvoiceId is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "InputInvoiceId must be a positive number.")]
    public int InputInvoiceId { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "InputInvoiceId must be a positive number.")]
    public int? ProductionBatchId { get; set; }
    
    public virtual InputInvoice InputInvoice { get; set; }
    public virtual ProductionBatch? ProductionBatch { get; set; }
    
    
    

    [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
    [Range(typeof(DateTime), "1900-01-01", "2024-12-31", ErrorMessage = "CreatedAt must be between 1900 and 2024.")]
    public DateTime CreatedAt { get; set; }

    [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
    [Range(typeof(DateTime), "1900-01-01", "2024-12-31", ErrorMessage = "ModifiedAt must be between 1900 and 2024.")]
    public DateTime? ModifiedAt { get; set; }

    [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
    [Range(typeof(DateTime), "1900-01-01", "2024-12-31", ErrorMessage = "RemovedAt must be between 1900 and 2024.")]
    public DateTime? RemovedAt { get; set; }

    [DataType(DataType.Date, ErrorMessage = "Invalid date format.")]
    [Range(typeof(DateTime), "1900-01-01", "2024-12-31", ErrorMessage = "RestoredAt must be between 1900 and 2024.")]
    public DateTime? RestoredAt { get; set; }
    
    

    [Range(1, int.MaxValue, ErrorMessage = "CreatedById must be a positive number.")]
    public int CreatedById { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "ModifiedById must be a positive number.")]
    public int? ModifiedById { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "RemovedById must be a positive number.")]
    public int? RemovedById { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "RestoreById must be a positive number.")]
    public int? RestoreById { get; set; }

    
    
    public virtual Employee CreatedBy { get; set; }
    public virtual Employee? ModifiedBy { get; set; }
    public virtual Employee? RemovedBy { get; set; }
    public virtual Employee? RestoreBy { get; set; }
}