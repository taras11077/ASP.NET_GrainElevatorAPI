using System.ComponentModel.DataAnnotations;

namespace GrainElevatorAPI.DTO.DTOs;

public class PriceListDto
{
    [Required(ErrorMessage = "Id is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "ProductWeight must be a positive number.")]
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Title is required.")]
    [MinLength(2, ErrorMessage = "Title must be at least 4 characters long.")]
    [MaxLength(20, ErrorMessage = "Title must be at least 20 characters long.")]
    public string ProductTitle { get; set; }
    
    public string CreatedByName { get; set; }
    
    public ICollection<CompletionReportDto> CompletionReports { get; set; } = new List<CompletionReportDto>();
    public ICollection<PriceListItemDto> PriceListItems { get; set; } = new List<PriceListItemDto>();
    
    public bool? IsFinalized { get; set; }
}