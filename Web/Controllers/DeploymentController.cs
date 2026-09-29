using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using Web.Mappers;
using Web.Request;
using Web.Response;

namespace Web.Controllers;

//[Authorize]
[ApiController]
[Route("api/[controller]")]
[EnableRateLimiting("external-api")]
public class DeploymentController : ControllerBase
{
    private readonly DeploymentMapper _deploymentMapper;
    
    public DeploymentController(DeploymentMapper deploymentMapper)
    {
        _deploymentMapper = deploymentMapper;
    }
    
    [HttpGet("")]
    public async Task<List<DeploymentBasicResponse>> GetAllAsync([FromQuery] Guid? incidentId, [FromQuery] Guid? teamId)
    {
        return await _deploymentMapper.GetAllAsync(incidentId, teamId);
    }
    
    [HttpGet("{id}")]
    public async Task<DeploymentResponse> GetAsync([FromQuery] Guid id)
    {
        return await _deploymentMapper.GetAsync(id);
    }
    
    [HttpPost("deploy")]
    public async Task<IActionResult> DeployAsync([FromBody] DeploymentRequest request)
    {
        var result = await _deploymentMapper.DeployAsync(request);
        return Ok(result);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync([FromQuery] Guid id, [FromBody] DeploymentUpdateRequest request)
    {
        var result = await _deploymentMapper.UpdateAsync(id, request);
        return Ok(result);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute]Guid id)
    {
        await _deploymentMapper.DeleteAsync(id);
        return Ok();
    }
    
    [HttpGet("paged")]
    public async Task<PaginatedResponse<DeploymentResponse>> GetAllPaged([FromQuery] PaginatedRequest request)
    {
        return await _deploymentMapper.GetAllPaginatedAsync(request);
    }
}