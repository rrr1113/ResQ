using Domain.Common;
using Domain.Enums;

namespace Domain.Models;

public class StatusUpdate : BaseAuditableEntity<Operator>
{
    public Guid IncidentId { get; set; }
    public Incident Incident { get; set; } = null!;

    public IncidentStatus Status { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    public string? Note { get; set; }

    public String? UpdatedByOperatorId { get; set; }
    public Operator? UpdatedByOperator { get; set; }
}