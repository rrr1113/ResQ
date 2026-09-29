using Domain.Enums;
using Microsoft.AspNetCore.Mvc;
using Web.Mappers;
using Web.Request;
using Web.Response;

namespace Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LocationController : ControllerBase
{
    private readonly LocationMapper _locationMapper;
    
    public LocationController(LocationMapper locationMapper)
    {
        _locationMapper = locationMapper;
    }
    
    [HttpGet("")]
    public async Task<List<LocationResponse>> GetAllAsync()
    {
        return await _locationMapper.GetAllAsync();
    }
    
    [HttpGet("{id}")]
    public async Task<LocationResponse> GetAsync([FromRoute]Guid id)
    {
        return await _locationMapper.GetAsync(id);
    }
    
    [HttpPost("insert")]
    public async Task<IActionResult> InsertTeam([FromBody] LocationRequest request)
    {
        var result = await _locationMapper.InsertAsync(request);
        return Ok(result);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute]Guid id)
    {
        await _locationMapper.DeleteAsync(id);
        return Ok();
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync([FromRoute]Guid id, [FromBody] LocationRequest request)
    {
        await  _locationMapper.UpdateAsync(id, request);
        return Ok();
    }
    
}