namespace GrainElevatorAPI.Core.Models
{
    public class AppDefect
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public string? CompanyName { get; set; }
        public bool Status { get; set; }
    }
}

