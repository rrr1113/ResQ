using Domain.Enums;
using Service.Interface;
using Web.Extensions;
using Web.Request;
using Web.Response;

namespace Web.Mappers;

public class EmergencyServiceMapper
{
    private readonly IEmergencyServiceService _emergencyServiceService;
    
    public EmergencyServiceMapper(IEmergencyServiceService emergencyServiceService)
    {
        _emergencyServiceService = emergencyServiceService;
    }
    
    public async Task<List<EmergencyServiceBasicResponse>> GetAllAsync()
    {
        var result = await _emergencyServiceService.GetAllAsync();
        return result.ToResponse();
    }
    
    public async Task<EmergencyServiceResponse> GetAsync(Guid deploymentId)
    {
        var result = await _emergencyServiceService.GetByIdNotNullAsync(deploymentId);
        return result.ToResponse();
    }
    
    public async Task<EmergencyServiceResponse> ReportAsync(EmergencyServiceRequest request)
    {
        var result = await _emergencyServiceService.InsertAsync(request.Name, Enum.Parse<ServiceType>(request.ServiceType), request.ContactPhone,  request.ContactEmail);
        return result.ToResponse();
    }
    
    public async Task<EmergencyServiceResponse> UpdateAsync(Guid id, EmergencyServiceRequest request)
    {
        var result = await _emergencyServiceService.UpdateAsync(id, request.Name, Enum.Parse<ServiceType>(request.ServiceType), request.ContactPhone,  request.ContactEmail);
        return result.ToResponse(); 
    }
    
    public async Task DeleteAsync(Guid id)
    {
        await _emergencyServiceService.DeleteByIdAsync(id);
    }
}