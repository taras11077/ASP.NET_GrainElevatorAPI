using GrainElevatorAPI.Core.Models;
using GrainElevatorAPI.DTO.Requests.UpdateRequests;
using GrainElevatorAPI.DTOs.Requests;

namespace GrainElevatorAPI.Extensions;

public static class LaboratoryCardExtensions
{
    public static void UpdateFromRequest(this LaboratoryCard laboratoryCard, LaboratoryCardUpdateRequest request)
    {
        laboratoryCard.LabCardNumber = request.LabCardNumber ?? laboratoryCard.LabCardNumber;
        laboratoryCard.WeedImpurity = request.WeedImpurity ?? laboratoryCard.WeedImpurity;
        laboratoryCard.Moisture = request.Moisture ?? laboratoryCard.Moisture;
        laboratoryCard.GrainImpurity = request.GrainImpurity ?? laboratoryCard.GrainImpurity;
        laboratoryCard.SpecialNotes = request.SpecialNotes ?? laboratoryCard.SpecialNotes;
        laboratoryCard.IsProduction = request.IsProduction ?? laboratoryCard.IsProduction;
    }
}