using Service.Interface;
using Web.Extensions;
using Web.Request;
using Web.Response;

namespace Web.Mappers;

public class LocationMapper
{
    private readonly ILocationService _locationService;

    public LocationMapper(ILocationService locationService)
    {
        _locationService = locationService;
    }
    
    public async Task<List<LocationResponse>> GetAllAsync()
    {
        var result = await _locationService.GetAllAsync();
        return result.ToResponse();
    }
    
    public async Task<LocationResponse> GetAsync(Guid id)
    {
        var result = await _locationService.GetByIdNotNullAsync(id);
        return result.ToResponse();
    }
    
    public async Task<LocationResponse> InsertAsync(LocationRequest request)
    {
        var result = await _locationService.InsertAsync(request.Address, request.City, request.Country, request.Latitude, request.Longitude);
        return result.ToResponse();
    }
    
    public async Task DeleteAsync(Guid id)
    {
        await _locationService.DeleteByIdAsync(id);
    }
    
    public async Task<LocationResponse> UpdateAsync(Guid id, LocationRequest request)
    {
        var result = await _locationService.UpdateAsync(id, request.Address, request.City, request.Country, request.Latitude.Value, request.Longitude.Value);
        return result.ToResponse();
    }
}