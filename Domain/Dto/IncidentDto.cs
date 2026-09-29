using Domain.Enums;

namespace Domain.Dto;

public class IncidentDto
{
    public IncidentType Type { get; set; }
    public string Description { get; set; }
    public int NumberOfInjured { get; set; }
    public PriorityLevel Priority { get; set; }
    public IncidentStatus Status { get; set; }
    
    public DateTime LastStatusUpdate { get; set; }
    public Guid LocationId { get; set; }
}