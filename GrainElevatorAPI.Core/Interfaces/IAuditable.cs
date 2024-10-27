using GrainElevatorAPI.Core.Models;

namespace GrainElevatorAPI.Core.Interfaces;

public interface IAuditable
{
    DateTime CreatedAt { get; set; }
    DateTime? ModifiedAt { get; set; }
    DateTime? RemovedAt { get; set; }
    DateTime? RestoredAt { get; set; }
    
    int? CreatedById { get; set; }
    int? ModifiedById { get; set; }
    int? RemovedById { get; set; }
    int? RestoreById { get; set; }
    
    Employee CreatedBy { get; set; }
    Employee? ModifiedBy { get; set; }
    Employee? RemovedBy { get; set; }
    Employee? RestoreBy { get; set; }
}
