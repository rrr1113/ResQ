using Domain.Models;

namespace Service.Interface;

public interface ITeamAssigmentService
{
    Task<List<Deployment>> AssignTeamsForIncidentAsync(Guid incidentId);
}