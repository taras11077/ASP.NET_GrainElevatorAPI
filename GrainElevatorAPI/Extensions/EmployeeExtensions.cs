using GrainElevatorAPI.Core.Models;
using GrainElevatorAPI.DTO.Requests.UpdateRequests;
using GrainElevatorAPI.DTOs.Requests;

namespace GrainElevatorAPI.Extensions;

public static class EmployeeExtensions
{
    public static void UpdateFromRequest(this Employee employee, EmployeeUpdateRequest request)
    {
        employee.FirstName = request.FirstName ?? employee.FirstName;
        employee.LastName = request.LastName ?? employee.LastName;
        employee.BirthDate = request.BirthDate ?? employee.BirthDate;
        employee.Email = request.Email ?? employee.Email;
        employee.Phone = request.Phone ?? employee.Phone;
        employee.Gender = request.Gender ?? employee.Gender;
        employee.City = request.City ?? employee.City;
        employee.Country = request.Country ?? employee.Country;
        employee.PasswordHash = request.PasswordHash ?? employee.PasswordHash;
    }
}
