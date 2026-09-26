using Domain.Common;
using Domain.Enums;

namespace Domain.Models;

public class Incident : BaseAuditableEntity<Operator>
{
    public IncidentType Type { get; set; }
    public string Description { get; set; } = string.Empty;
    public int NumberOfInjured { get; set; }
    public DateTime ReportedAt { get; set; } = DateTime.UtcNow;
    public PriorityLevel? Priority { get; set; }
    public IncidentStatus Status { get; set; } = IncidentStatus.Reported;
    
    public Guid LocationId { get; set; }
    public Location Location { get; set; } = null!;

    public Guid OperatorId { get; set; }
    public Operator Operator { get; set; } = null!;

    public ICollection<Deployment> Deployments { get; set; } = new List<Deployment>();
    public ICollection<StatusUpdate> StatusUpdates { get; set; } = new List<StatusUpdate>();
}