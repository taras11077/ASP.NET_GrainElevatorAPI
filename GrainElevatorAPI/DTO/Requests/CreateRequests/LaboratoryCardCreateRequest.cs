using System.ComponentModel.DataAnnotations;

namespace GrainElevatorAPI.DTO.Requests.CreateRequests;

public class LaboratoryCardCreateRequest
{
    [Required(ErrorMessage = "InputInvoiceId is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "InputInvoiceId must be a positive number.")]
    public int InputInvoiceId { get; set; }
    
    
    [Required(ErrorMessage = "LabCardNumber is required.")]
    [MinLength(3, ErrorMessage = "LaboratoryCard Number must be at least 3 characters long.")]
    public string LabCardNumber { get; set; }
    
    [Required(ErrorMessage = "WeedImpurity is required.")]
    [Range(0.0, 100.0, ErrorMessage = "WeedImpurity must be between 0.0 and 100.0")]
    public double WeedImpurity { get; set; }

    [Required(ErrorMessage = "Moisture is required.")]
    [Range(0.0, 100.0, ErrorMessage = "Moisture must be between 0.0 and 100.0")]
    public double Moisture { get; set; }

    [Range(0.0, 100.0, ErrorMessage = "GrainImpurity must be between 0.0 and 100.0")]
    public double? GrainImpurity { get; set; }

    [MaxLength(300, ErrorMessage = "SpecialNotes must be at least 300 characters long.")]
    public string? SpecialNotes { get; set; }
    
    public bool? IsProduction { get; set; }

}