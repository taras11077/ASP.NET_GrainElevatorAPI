using System.ComponentModel.DataAnnotations;

namespace GrainElevatorAPI.DTO.DTOs;

public class PriceListDto
{
    [Required(ErrorMessage = "Id is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "ProductWeight must be a positive number.")]
    public int Id { get; set; }

    [Required(ErrorMessage = "Id is required.")]
    [Range(1, int.MaxValue, ErrorMessage = "ProductWeight must be a positive number.")]
    public int ProductId { get; set; }
    
    public ICollection<CompletionReportDto> CompletionReports { get; set; } = new List<CompletionReportDto>();
    public ICollection<PriceListItemDto> PriceListItems { get; set; } = new List<PriceListItemDto>();
    
    public bool? IsFinalized { get; set; }
}