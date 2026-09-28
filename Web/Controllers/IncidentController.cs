using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
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
    
    public IncidentController(IncidentMapper mapper)
    {
        _incidentMapper = mapper;
    }
    
    [HttpGet("")]
    public async Task<List<IncidentResponse>> GetAllAsync([FromQuery] string? city, [FromQuery] string? country)
    {
        return await _incidentMapper.GetAllAsync(city, country);
    }
    
    [HttpGet("paged")]
    public async Task<PaginatedResponse<IncidentResponse>> GetAllPaged([FromQuery] PaginatedRequest request)
    {
        return await _incidentMapper.GetAllPaginatedAsync(request);
    }
    
    [HttpPost("report")]
    public async Task<IActionResult> ReportAsync([FromBody] IncidentRequest request)
    {
        var result = await _incidentMapper.ReportAsync(request);
        return Ok(result);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute]Guid id)
    {
        await _incidentMapper.DeleteAsync(id);
        return Ok();
    }
    
    [HttpPatch("{id}/updateStatus")]
    public async Task<IActionResult> UpdateStatus([FromRoute]Guid id, [FromBody] IncidentStatus status)
    {
        await  _incidentMapper.UpdateStatus(id, status);
        return Ok();
    }
}