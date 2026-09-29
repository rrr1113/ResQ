using Microsoft.AspNetCore.Mvc;
using Service;


namespace Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeamAssignmentController : ControllerBase
{
    private readonly TeamAssignmentService _teamAssignmentService;

    public TeamAssignmentController(TeamAssignmentService teamAssignmentService)
    {
        _teamAssignmentService = teamAssignmentService;
    } 
    
    [HttpGet("{incidentId}")]
    public async Task<IActionResult> AutomaticDeployments([FromQuery] Guid incidentId)
    {
        var result = await _teamAssignmentService.AssignTeamsForIncidentAsync(incidentId);
        return Ok(result);
    }
}