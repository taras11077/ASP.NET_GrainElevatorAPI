using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Interfaces;

public interface IRegisterService
{
    Task<Register> AddRegisterAsync(Register register);
    Task<Register> GetRegisterByIdAsync(int id);
    Task<Register> UpdateRegisterAsync(Register register);
    Task<bool> DeleteRegisterAsync(int id);
    IQueryable<Register> GetRegisters(int page, int size);
    IEnumerable<Register> SearchLRegisters(string registerNumber);
}