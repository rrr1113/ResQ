namespace Domain.Dto;

public class DeploymentDto
{
    public DateTime DispatchTime { get; set; }
    public DateTime? ArrivalTime { get; set; }
    public DateTime? CompletionTime { get; set; }
    public string? Notes { get; set; }
    
    public Guid IncidentId { get; set; }
    public Guid ResponseTeamId { get; set; }
    public Guid VehicleId { get; set; }
}