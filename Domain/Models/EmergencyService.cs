using Domain.Common;
using Domain.Enums;

namespace Domain.Models;

public class EmergencyService : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public ServiceType ServiceType { get; set; }
    public string ContactPhone { get; set; } = string.Empty;
    public string ContactEmail { get; set; } = string.Empty;

    public ICollection<ResponseTeam> ResponseTeams { get; set; } = new List<ResponseTeam>();
}