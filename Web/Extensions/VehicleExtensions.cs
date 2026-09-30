using Domain.Models;
using Web.Response;

namespace Web.Extensions;

public static class VehicleExtensions
{
    public static VehicleResponse ToResponse(this Vehicle vehicle)
    {
        return new VehicleResponse(
            vehicle.Id,
            vehicle.PlateNumber,
            vehicle.VehicleType.ToString(),
            vehicle.ResponseTeam.Name,
            vehicle.Status.ToString(),
            vehicle.Capacity
        );
    }
    
    public static List<VehicleResponse> ToResponse(this List<Vehicle> vehicles)
    {
        return vehicles.Select(x => x.ToResponse()).ToList();
    }
}