
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Service.Interface;
using Web.Mappers;
using Web.Request;
using Web.Response;

namespace Web.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
[EnableRateLimiting("external-api")]
public class IncidentController : ControllerBase
{
    private readonly IncidentMapper _incidentMapper;
    private readonly IDeploymentService _deploymentService;
    
    public IncidentController(IncidentMapper mapper, IDeploymentService deploymentService)
    {
        _incidentMapper = mapper;
        _deploymentService = deploymentService;
    }
    
    [HttpGet("")]
    public async Task<List<IncidentResponse>> GetAllAsync([FromQuery] string? city, [FromQuery] string? country)
    {
        return await _incidentMapper.GetAllAsync(city, country);
    }
    
    [HttpGet("{id}")]
    public async Task<IncidentResponse> GetAsync([FromRoute] Guid id)
    {
        return await _incidentMapper.GetAsync(id);
    }
    
    [HttpPost("report")]
    public async Task<IActionResult> ReportAsync([FromBody] IncidentRequest request)
    {
        var result = await _incidentMapper.ReportAsync(request);
        return Ok(result);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromBody] IncidentUpdateRequest request)
    {
        var result = await _incidentMapper.UpdateAsync(id, request);
        return Ok(result);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute]Guid id)
    {
        await _incidentMapper.DeleteAsync(id);
        return Ok();
    }
    
    [HttpPatch("{id}/updateStatus")]
    public async Task<IActionResult> UpdateStatus([FromRoute]Guid id, [FromBody] String status)
    {
        await  _incidentMapper.UpdateStatus(id, Enum.Parse<IncidentStatus>(status));
        await _deploymentService.HandleIncidentStatusChange(id,  Enum.Parse<IncidentStatus>(status));
        return Ok();
    }
    
    [HttpGet("paged")]
    public async Task<PaginatedResponse<IncidentResponse>> GetAllPaged([FromQuery] PaginatedRequest request)
    {
        return await _incidentMapper.GetAllPaginatedAsync(request);
    }
}