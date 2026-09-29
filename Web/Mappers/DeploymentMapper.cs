using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Dto;
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
    
    public async Task<DeploymentResponse> GetAsync(Guid deploymentId)
    {
        var result = await _deploymentService.GetByIdNotNullAsync(deploymentId);
        
        return result.ToResponse();
    }

    public async Task<DeploymentResponse> DeployAsync(DeploymentRequest request)
    {
        var result = await _deploymentService.InsertAsync(request.IncidentId, request.ResponseTeamId, request.VehicleId, request.Notes);
        return result.ToResponse();
    }
    
    public async Task DeleteAsync(Guid id)
    {
        await _deploymentService.DeleteByIdAsync(id);
    }
    
    public async Task<DeploymentResponse> UpdateAsync(Guid id, DeploymentUpdateRequest request)
    {
        DeploymentDto deploymentDto = request.ToDto();
        var result = await _deploymentService.UpdateAsync(id, deploymentDto);
        return result.ToResponse();
    }
    
    public async Task<PaginatedResponse<DeploymentResponse>> GetAllPaginatedAsync(PaginatedRequest request)
    {
        var result = await _deploymentService.GetPagedAsync(request.PageNumber, request.PageSize);
        return result.ToPaginatedResponse(x => x.ToResponse());
    }
}