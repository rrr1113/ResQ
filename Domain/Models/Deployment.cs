using Domain.Common;

namespace Domain.Models;

public class Deployment : BaseEntity
{
    public DateTime DispatchTime { get; set; } = DateTime.UtcNow;
    public DateTime? ArrivalTime { get; set; }
    public DateTime? CompletionTime { get; set; }
    public string? Notes { get; set; }
    
    public Guid IncidentId { get; set; }
    public Incident Incident { get; set; } = null!;

    public Guid ResponseTeamId { get; set; }
    public ResponseTeam ResponseTeam { get; set; } = null!;

    public Guid VehicleId { get; set; }
    public Vehicle Vehicle { get; set; } = null!;
    
    public TimeSpan? ResponseTime => ArrivalTime.HasValue ? ArrivalTime - DispatchTime : null;
    public TimeSpan? TotalDuration => CompletionTime.HasValue ? CompletionTime - DispatchTime : null;
}