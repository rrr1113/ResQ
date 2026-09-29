using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using Web.Extensions;
using Web.Request;
using Web.Response;

namespace Web.Mappers;

public class IncidentMapper
{
    private readonly IIncidentService _incidentService;
    private readonly ILocationService _locationService;
    private readonly IOperatorService _operatorService;
    
    public IncidentMapper(IIncidentService incidentService, IOperatorService operatorService, ILocationService locationService)
    {
        _incidentService = incidentService;
        _operatorService = operatorService;
        _locationService = locationService;
    }
    
    public async Task<List<IncidentResponse>> GetAllAsync(string? city, string? country)
    {
        var result = await _incidentService.GetAllAsync(city, country);
        return result.ToResponse();
    }
    
    public async Task<IncidentResponse> GetAsync(Guid deploymentId)
    {
        var result = await _incidentService.GetByIdNotNullAsync(deploymentId);
        return result.ToResponse();
    }
    
    public async Task<IncidentBasicResponse> ReportAsync(IncidentRequest request)
    {
        var location = await _locationService.InsertAsync(request.Address, request.City, request.Country);
        
        var result = await _incidentService.InsertAsync(Enum.Parse<IncidentType>(request.Type, true), request.Description, request.NumberOfInjured, location.Id);
        return result.ToBasicResponse();
    }
    
    public async Task<IncidentResponse> UpdateAsync(Guid id, IncidentUpdateRequest request)
    {
        var incidentDto = request.ToDto();
        var result = await _incidentService.UpdateAsync(id, incidentDto);
        return result.ToResponse(); 
    }
    
    public async Task UpdateStatus(Guid incidentId, IncidentStatus status)
    {
        var incident = await _incidentService.GetByIdNotNullAsync(incidentId);
        var operatorId = _operatorService.GetUserId()!;

        if (incident.OperatorId != operatorId)
        {
            throw new InvalidOperationException("Cannot change status of another incident");
        }
        
        await _incidentService.UpdateStatus(incidentId, status);
    }
    
    public async Task DeleteAsync(Guid id)
    {
        await _incidentService.DeleteByIdAsync(id);
    }
    
    public async Task<PaginatedResponse<IncidentResponse>> GetAllPaginatedAsync(PaginatedRequest request)
    {
        var result = await _incidentService.GetPagedAsync(request.PageNumber, request.PageSize);
        return result.ToPaginatedResponse(x => x.ToResponse());
    }
}