using Domain.Common;
using Domain.Enums;

namespace Domain.Models;

public class Vehicle : BaseEntity
{
    public string PlateNumber { get; set; } = string.Empty;
    public VehicleType VehicleType { get; set; }
    public VehicleStatus Status { get; set; } = VehicleStatus.Available;

    public Guid ResponseTeamId { get; set; }
    public ResponseTeam ResponseTeam { get; set; } = null!;

    public ICollection<Deployment> Deployments { get; set; } = new List<Deployment>();
}