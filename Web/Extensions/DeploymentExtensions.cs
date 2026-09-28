using Domain.Models;
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
}