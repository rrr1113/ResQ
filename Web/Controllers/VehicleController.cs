using Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Web.Mappers;
using Web.Request;
using Web.Response;

namespace Web.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class VehicleController : ControllerBase
{
    private readonly VehicleMapper _vehicleMapper;
    
    public VehicleController(VehicleMapper vehicleMapper)
    {
        _vehicleMapper = vehicleMapper;
    }
    
    [HttpGet("")]
    public async Task<List<VehicleResponse>> GetAllAsync()
    {
        return await _vehicleMapper.GetAllAsync();
    }
    
    [HttpGet("{id}")]
    public async Task<VehicleResponse> GetAsync([FromRoute]Guid id)
    {
        return await _vehicleMapper.GetAsync(id);
    }
    
    [HttpPost("insert")]
    public async Task<IActionResult> InsertVehicle([FromBody] VehicleRequest request)
    {
        var result = await _vehicleMapper.InsertAsync(request);
        return Ok(result);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute]Guid id)
    {
        await _vehicleMapper.DeleteAsync(id);
        return Ok();
    }
    
    [HttpPatch("{id}/updateStatus")]
    public async Task<IActionResult> UpdateStatus([FromRoute]Guid id, [FromBody] String status)
    {
        await  _vehicleMapper.UpdateStatus(id, Enum.Parse<VehicleStatus>(status));
        return Ok();
    }
}