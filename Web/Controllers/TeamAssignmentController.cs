using Microsoft.AspNetCore.Mvc;
using Service.Interface;
using Web.Extensions;
using Web.Response;


namespace Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeamAssignmentController : ControllerBase
{
    private readonly ITeamAssigmentService _teamAssignmentService;

    public TeamAssignmentController(ITeamAssigmentService teamAssignmentService)
    {
        _teamAssignmentService = teamAssignmentService;
    } 
    
    [HttpGet("{incidentId}")]
    public async Task<List<DeploymentBasicResponse>> AutomaticDeployments([FromRoute] Guid incidentId)
    {
        var result = await _teamAssignmentService.AssignTeamsForIncidentAsync(incidentId);
        return result.ToResponse();
    }
}