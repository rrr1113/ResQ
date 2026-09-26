using Domain.Common;
using Domain.Enums;

namespace Domain.Models;

public class ResponseTeam : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public int NumberOfMembers { get; set; }
    public TeamStatus Status { get; set; } = TeamStatus.Available;
    
    public Guid EmergencyServiceId { get; set; }
    public EmergencyService EmergencyService { get; set; } = null!;

    public Guid BaseLocationId { get; set; }
    public Location BaseLocation { get; set; } = null!;

    public ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
    public ICollection<Deployment> Deployments { get; set; } = new List<Deployment>();
}