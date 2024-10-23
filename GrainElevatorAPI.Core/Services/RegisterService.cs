using GrainElevatorAPI.Core.Interfaces;
using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Services;

public class RegisterService : IRegisterService
{
    private readonly IRepository _repository;

    public RegisterService(IRepository repository) => _repository = repository;
    
    public async Task<Register> AddRegisterAsync(Register register)
    {
        try
        {
            return await _repository.Add(register);
        }
        catch (Exception ex)
        {
            throw new Exception("Помилка при додаванні Реєстру", ex);
        }
    }

    public async Task<Register> GetRegisterByIdAsync(int id)
    {
        try
        {
            return await _repository.GetById<Register>(id);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка при отриманні Реєстру з ID {id}", ex);
        }
    }

    public async Task<Register> UpdateRegisterAsync(Register register)
    {
        try
        {
            return await _repository.Update(register);
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка при оновленні Реєстру з ID  {register.Id}", ex);
        }
    }

    public async Task<bool> DeleteRegisterAsync(int id)
    {
        try
        {
            var register = await _repository.GetById<Register>(id);
            if (register != null)
            {
                await _repository.Delete<Register>(id);
                return true;
            }

            return false;
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка при видаленні Реєстру з ID {id}", ex);
        }
    }

    public IQueryable<Register> GetRegisters(int page, int size)
    {
        try
        {
            return _repository.GetAll<Register>()
                .Skip((page - 1) * size)
                .Take(size);
        }
        catch (Exception ex)
        {
            throw new Exception("Помилка при отриманні списку Реєстрів", ex);
        }
    }

    public IEnumerable<Register> SearchLRegisters(string registerNumber)
    {
        try
        {
            return _repository.GetAll<Register>()
                .Where(r => r.RegisterNumber.ToLower().Contains(registerNumber.ToLower()))
                .ToList();
        }
        catch (Exception ex)
        {
            throw new Exception($"Помилка при отриманні Реєстру за номером {registerNumber}", ex);
        }
    }
}