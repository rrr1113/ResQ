using Service.Interface;
using Web.Extensions;
using Web.Request;
using Web.Response;

namespace Web.Mappers;

public class DeploymentMapper
{
    private readonly IDeploymentService _deploymentService;

    public DeploymentMapper(IDeploymentService deploymentService)
    {
        _deploymentService = deploymentService;
    }
    
    public async Task<List<DeploymentBasicResponse>> GetAllAsync(Guid? incidentId, Guid? teamId)
    {
        var result = await _deploymentService.GetAllAsync(incidentId,  teamId);
        
        return result.ToResponse();
    }

    public async Task<DeploymentBasicResponse> DeployAsync(DeploymentRequest request)
    {
        var result = await _deploymentService.InsertAsync(request.IncidentId, request.ResponseTeamId, request.VehicleId, request.Notes);
        return result.ToBasicResponse();
    }
    
    public async Task<List<DeploymentBasicResponse>> AutoDispatchAsync(Guid incidentId)
    {
        var deployments = await _deploymentService.AssignTeamsForIncidentAsync(incidentId);
        return deployments.ToResponse();
    }
    
    public async Task DeleteAsync(Guid id)
    {
        await _deploymentService.DeleteByIdAsync(id);
    }
    
    public async Task<PaginatedResponse<DeploymentResponse>> GetAllPaginatedAsync(PaginatedRequest request)
    {
        var result = await _deploymentService.GetPagedAsync(request.PageNumber, request.PageSize);
        return result.ToPaginatedResponse(x => x.ToResponse());
    }
}