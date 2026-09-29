using Domain.Dto;
using Domain.Models;
using Web.Request;
using Web.Response;

namespace Web.Extensions;

public static class DeploymentExtensions
{
    public static DeploymentResponse ToResponse(this Deployment deployment)
    {
        return new DeploymentResponse(
            deployment.DispatchTime,
            deployment.ArrivalTime,
            deployment.CompletionTime,
            deployment.Incident.Type.ToString(),
            deployment.Incident.Location.Address + " - " + deployment.Incident.Location.City,
            deployment.ResponseTeam.Name,
            deployment.Vehicle.PlateNumber
        );
    }
    
    public static List<DeploymentBasicResponse> ToResponse(this List<Deployment> deployments)
    {
        return deployments.Select(x => x.ToBasicResponse()).ToList();
    }
    
    public static DeploymentBasicResponse ToBasicResponse(this Deployment deployment)
    {
        return new DeploymentBasicResponse(
            deployment.DispatchTime,
            deployment.ArrivalTime,
            deployment.Incident.Location.Address + " - " + deployment.Incident.Location.City,
            deployment.ResponseTeam.Name,
            deployment.Vehicle.PlateNumber
        );
    }

    public static DeploymentDto ToDto(this DeploymentUpdateRequest deployment)
    {
        return new DeploymentDto
        {
            ArrivalTime = deployment.ArrivalTime,
            CompletionTime = deployment.CompletionTime,
            DispatchTime = deployment.DispatchTime,
            Notes = deployment.Notes,
            IncidentId = deployment.IncidentId,
            ResponseTeamId = deployment.ResponseTeamId,
            VehicleId = deployment.VehicleId
        };
    }
}