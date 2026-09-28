using Domain.Models;
using Web.Response;

namespace Web.Extensions;

public static class IncidentExtensions
{
    public static IncidentResponse ToResponse(this Incident incident)
    {
        return new IncidentResponse(
            incident.Type,
            incident.ReportedAt,
            incident.Location.Address,
            incident.Location.City,
            incident.Priority?.ToString(),
            incident.Status.ToString(),
            incident.Deployments.Select(d => d.ResponseTeam.ToBasicResponse()).ToList()
        );
    }
    
    public static List<IncidentResponse> ToResponse(this List<Incident> incidents)
    {
        return incidents.Select(x => x.ToResponse()).ToList();
    }
    
    public static IncidentBasicResponse ToBasicResponse(this Incident incident)
    {
        return new IncidentBasicResponse(
            incident.Type.ToString(),
            incident.ReportedAt,
            incident.Location.Address,
            incident.Location.City
        );
    }
}