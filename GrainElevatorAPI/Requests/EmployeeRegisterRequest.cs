namespace GrainElevatorAPI.Requests;
using System.ComponentModel.DataAnnotations;

public class EmployeeRegisterRequest : EmployeeLoginRequest
{
    public int RoleId { get; set; }
}