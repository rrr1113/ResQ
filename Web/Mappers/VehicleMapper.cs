using Domain.Enums;
using Service.Interface;
using Web.Extensions;
using Web.Request;
using Web.Response;

namespace Web.Mappers;

public class VehicleMapper
{
    private readonly IVehicleService _vehicleService;

    public VehicleMapper(IVehicleService vehicleService)
    {
        _vehicleService = vehicleService;
    }
    
    public async Task<List<VehicleResponse>> GetAllAsync()
    {
        var result = await _vehicleService.GetAllAsync();
        return result.ToResponse();
    }
    
    public async Task<VehicleResponse> GetAsync(Guid id)
    {
        var result = await _vehicleService.GetByIdNotNullAsync(id);
        return result.ToResponse();
    }
    
    public async Task<VehicleResponse> InsertAsync(VehicleRequest request)
    {
        var result = await _vehicleService.InsertAsync(request.PlateNumber, Enum.Parse<VehicleType>(request.VehicleType), request.ResponseTeamId);
        return result.ToResponse();
    }
    
    public async Task DeleteAsync(Guid id)
    {
        await _vehicleService.DeleteByIdAsync(id);
    }
    
    public async Task UpdateStatus(Guid incidentId, VehicleStatus status)
    {
        await _vehicleService.UpdateStatus(incidentId, status);
    }
}