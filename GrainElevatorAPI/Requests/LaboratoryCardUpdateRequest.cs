using System.ComponentModel.DataAnnotations;

namespace GrainElevatorAPI.Requests;

public class LaboratoryCardUpdateRequest
{
    [MinLength(3, ErrorMessage = "LaboratoryCard Number must be at least 3 characters long.")]
    [MaxLength(9, ErrorMessage = "LaboratoryCard Number must be at least 9 characters long.")]
    public string? LabCardNumber { get; set; }
    
    [Range(0.0, 100.0, ErrorMessage = "WeedImpurity must be between 0.0 and 100.0")]
    public double? WeedImpurity { get; set; }
    
    [Range(0.0, 100.0, ErrorMessage = "Moisture must be between 0.0 and 100.0")]
    public double? Moisture { get; set; }

    [Range(0.0, 100.0, ErrorMessage = "GrainImpurity must be between 0.0 and 100.0")]
    public double? GrainImpurity { get; set; }

    [MaxLength(300, ErrorMessage = "SpecialNotes must be at least 300 characters long.")]
    public string? SpecialNotes { get; set; }
    
    public bool? IsProduction { get; set; }
}