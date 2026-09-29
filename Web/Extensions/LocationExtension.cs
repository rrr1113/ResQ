using Domain.Models;
using Web.Response;

namespace Web.Extensions;

public static class LocationExtension
{
    public static LocationResponse ToResponse(this Location location)
    {
        return new LocationResponse(
            location.Id,
            location.Address,
            location.City,
            location.Country,
            location.Longitude,
            location.Latitude
        );
    }
    
    public static List<LocationResponse> ToResponse(this List<Location> locations)
    {
        return locations.Select(x => x.ToResponse()).ToList();
    }
    
    public static LocationBasicResponse ToBasicResponse(this Location location)
    {
        return new LocationBasicResponse(
            location.Address,
            location.City,
            location.Country
        );
    }
}