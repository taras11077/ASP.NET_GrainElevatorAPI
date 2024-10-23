using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;

public interface ILaboratoryCardService
{
    Task<LaboratoryCard> AddLaboratoryCardAsync(LaboratoryCard laboratoryCard, int createdById);
    Task<LaboratoryCard> GetLaboratoryCardByIdAsync(int id);
    Task<LaboratoryCard> UpdateLaboratoryCardAsync(LaboratoryCard laboratoryCard, int modifiedById);
    Task<LaboratoryCard> SoftDeleteLaboratoryCardAsync(LaboratoryCard laboratoryCard, int removedById);
    Task<LaboratoryCard> RestoreRemovedLaboratoryCardAsync(LaboratoryCard laboratoryCard, int restoredById);
    Task<bool> DeleteLaboratoryCardAsync(int id);
    IQueryable<LaboratoryCard> GetLaboratoryCards(int page, int size);
    IEnumerable<LaboratoryCard> SearchLaboratoryCards(int? id,
        string? labCardNumber,
        double? weedImpurity,
        double? moisture,
        bool? isProduction,
        DateTime? arrivalDate,
        string? vehicleNumber,
        int? physicalWeight,
        int? supplierId,
        int? productId,
        int? createdById,
        DateTime? removedAt,
        int page,
        int size);
}