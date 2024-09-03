using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Interfaces;

public interface ILaboratoryCardService
{
    Task<LaboratoryCard> AddLaboratoryCardAsync(LaboratoryCard laboratoryCard);
    Task<LaboratoryCard> GetLaboratoryCardByIdAsync(int id);
    Task<LaboratoryCard> UpdateLaboratoryCardAsync(LaboratoryCard laboratoryCard);
    Task<bool> DeleteLaboratoryCardAsync(int id);
    IQueryable<LaboratoryCard> GetLaboratoryCards(int page, int size);
    IEnumerable<LaboratoryCard> SearchLaboratoryCards(string labCardNumber);
}