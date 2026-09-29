using Domain.Enums;

namespace Domain.Dto;

public class UpdateVehicleDto
{
    public Guid Id { get; set; }
    public String PlateNumber { get; set; }
    public VehicleType VehicleType { get; set; }
    public VehicleStatus Status  { get; set; }
    public Guid ResponseTeamId { get; set; }
}