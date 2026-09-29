using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Web.Mappers;
using Web.Request;
using Web.Response;

namespace Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmergencyServiceController : ControllerBase
{
    private readonly EmergencyServiceMapper _emergencyServiceMapper;
    
    public EmergencyServiceController(EmergencyServiceMapper emergencyServiceMapper)
    {
        _emergencyServiceMapper = emergencyServiceMapper;
    }
    
    [HttpGet("")]
    public async Task<List<EmergencyServiceResponse>> GetAllAsync()
    {
        return await _emergencyServiceMapper.GetAllAsync();
    }
    
    [HttpGet("{id}")]
    public async Task<EmergencyServiceResponse> GetAsync([FromRoute]Guid id)
    {
        return await _emergencyServiceMapper.GetAsync(id);
    }
    
    [HttpPost("add")]
    public async Task<IActionResult> ReportIncident([FromBody]EmergencyServiceRequest request)
    {
        var result = await _emergencyServiceMapper.ReportAsync(request);
        return Ok(result);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync([FromRoute]Guid id)
    {
        await _emergencyServiceMapper.DeleteAsync(id);
        return Ok();
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync([FromRoute]Guid id, [FromBody] EmergencyServiceRequest request)
    {
        await  _emergencyServiceMapper.UpdateAsync(id, request);
        return Ok();
    }
}