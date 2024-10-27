namespace GrainElevatorAPI.DTOs;

public class EmployeeDto
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public DateTime? BirthDate { get; set; }
    public string Email { get; set; }
    public string? Phone { get; set; }
    public string? Gender { get; set; }
    public string? City { get; set; }
    public int RoleId { get; set; }
}