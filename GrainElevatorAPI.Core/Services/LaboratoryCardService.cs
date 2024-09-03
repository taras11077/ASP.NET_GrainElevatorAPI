using GrainElevatorAPI.Core.Interfaces;
using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Services;

public class LaboratoryCardService : ILaboratoryCardService
{
    private readonly IRepository _repository;

    public LaboratoryCardService(IRepository repository) => _repository = repository;


    public async Task<LaboratoryCard> AddLaboratoryCardAsync(LaboratoryCard laboratoryCard)
    {
        try
        {
            return await _repository.Add(laboratoryCard);
        }
        catch (Exception ex)
        {
            throw new Exception("Помилка при додаванні Лабораторної карточки", ex);
        }
    }

    public async Task<LaboratoryCard> GetLaboratoryCardByIdAsync(int id)
    {
        try
        {
            return await _repository.GetById<LaboratoryCard>(id);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка при отриманні Лабораторної карточки з ID {id}", ex);
        }
    }

    public async Task<LaboratoryCard> UpdateLaboratoryCardAsync(LaboratoryCard laboratoryCard)
    {
        try
        {
            return await _repository.Update(laboratoryCard);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка при оновленні Лабораторної карточки з ID  {laboratoryCard.Id}", ex);
        }
    }

    public async Task<bool> DeleteLaboratoryCardAsync(int id)
    {
        try
        {
            var laboratoryCard = await _repository.GetById<LaboratoryCard>(id);
            if (laboratoryCard != null)
            {
                await _repository.Delete<LaboratoryCard>(id);
                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка при видаленні Лабораторної карточки з ID {id}", ex);
        }
    }

    public IQueryable<LaboratoryCard> GetLaboratoryCards(int page, int size)
    {
        try
        {
            return _repository.GetAll<LaboratoryCard>()
                .Skip((page - 1) * size)
                .Take(size);
        }
        catch (Exception ex)
        {
            throw new Exception("Помилка при отриманні списку Лабораторних карточок", ex);
        }
    }

    public IEnumerable<LaboratoryCard> SearchLaboratoryCards(string labCardNumber)
    {
        try
        {
            return _repository.GetAll<LaboratoryCard>()
                .Where(lc => lc.LabCardNumber.ToLower().Contains(labCardNumber.ToLower()))
                .ToList();
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка при отриманні Лабораторної карточки за номером {labCardNumber}", ex);
        }
    }
}