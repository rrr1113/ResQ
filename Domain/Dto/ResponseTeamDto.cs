using Domain.Enums;

namespace Domain.Dto;

public class ResponseTeamDto
{
    public string Name { get; set; } = string.Empty;
    public int NumberOfMembers { get; set; }
    public TeamStatus Status { get; set; }
    public Guid EmergencyServiceId { get; set; }
    public Guid BaseLocationId { get; set; }
}