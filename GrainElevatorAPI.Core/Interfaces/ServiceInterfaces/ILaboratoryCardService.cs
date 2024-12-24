using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Interfaces.ServiceInterfaces;

public interface ILaboratoryCardService
{
    Task<LaboratoryCard> CreateLaboratoryCardAsync(LaboratoryCard laboratoryCard, int createdById, CancellationToken cancellationToken);
    Task<LaboratoryCard> GetLaboratoryCardByIdAsync(int id, CancellationToken cancellationToken);
    Task<LaboratoryCard> UpdateLaboratoryCardAsync(LaboratoryCard laboratoryCard, int modifiedById, CancellationToken cancellationToken);
    Task<LaboratoryCard> SoftDeleteLaboratoryCardAsync(LaboratoryCard laboratoryCard, int removedById, CancellationToken cancellationToken);
    Task<LaboratoryCard> RestoreRemovedLaboratoryCardAsync(LaboratoryCard laboratoryCard, int restoredById, CancellationToken cancellationToken);
    Task<bool> DeleteLaboratoryCardAsync(int id, CancellationToken cancellationToken);
    Task<IEnumerable<LaboratoryCard>> GetLaboratoryCards(int page, int size, CancellationToken cancellationToken);
    Task<(IEnumerable<LaboratoryCard>, int)> SearchLaboratoryCards(
        string? labCardNumber,
        double? weedImpurity,
        double? moisture,
        bool? isProduction,
        DateTime? arrivalDate,
        string? vehicleNumber,
        int? physicalWeight,
        string? supplierTitle,
        string? productTitle,
        string? createdByName,
        DateTime? removedAt,
        int page,
        int size,
        string? sortField,
        string? sortOrder,
        CancellationToken cancellationToken);
}