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

[Authorize]
[ApiController]
[Route("api/[controller]")]
[EnableRateLimiting("external-api")]
public class ResponseTeamController : ControllerBase
{
    private readonly ResponseTeamMapper _responseTeamMapper;
    
    public ResponseTeamController(ResponseTeamMapper responseTeamMapper)
    {
        _responseTeamMapper = responseTeamMapper;
    }
    
    [HttpGet("")]
    public async Task<List<ResponseTeamResponse>> GetAllAsync()
    {
        return await _responseTeamMapper.GetAllAsync();
    }
    
    [HttpGet("{id}")]
    public async Task<ResponseTeamResponse> GetAsync([FromRoute]Guid id)
    {
        return await _responseTeamMapper.GetAsync(id);
    }
    
    [HttpPost("insert")]
    public async Task<IActionResult> InsertTeam([FromBody] ResponseTeamRequest request)
    {
        var result = await _responseTeamMapper.InsertAsync(request);
        return Ok(result);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute]Guid id)
    {
        await _responseTeamMapper.DeleteAsync(id);
        return Ok();
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync([FromRoute]Guid id, [FromBody] ResponseTeamRequest request)
    {
        await  _responseTeamMapper.UpdateAsync(id, request);
        return Ok();
    }
    
    [HttpPatch("{id}/updateStatus")]
    public async Task<IActionResult> UpdateStatus([FromRoute]Guid id, [FromBody] String status)
    {
        await  _responseTeamMapper.UpdateStatus(id, Enum.Parse<TeamStatus>(status));
        return Ok();
    }
}