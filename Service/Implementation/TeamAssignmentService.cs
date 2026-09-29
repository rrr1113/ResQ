using Domain.Dto.Email;
using Domain.Enums;
using Domain.Models;
using Service.Interface;

namespace Service;

public class TeamAssignmentService : ITeamAssigmentService
{
    private readonly IDeploymentService _deploymentService;
    private readonly IResponseTeamService _responseTeamService;
    private readonly IVehicleService _vehicleService;
    private readonly IEmailQueue _emailQueue;
    private readonly IIncidentService _incidentService;
    
    public TeamAssignmentService(IDeploymentService deploymentService,
     IResponseTeamService responseTeamService, IVehicleService vehicleService, 
     IEmailQueue emailQueue, 
     IIncidentService incidentService)
    {
        _deploymentService = deploymentService;
        _responseTeamService = responseTeamService;
        _vehicleService = vehicleService;
        _emailQueue = emailQueue;
        _incidentService = incidentService;
    }
    
     public async Task<List<Deployment>> AssignTeamsForIncidentAsync(Guid incidentId)
    {
        Incident incident = await _incidentService.GetByIdNotNullAsync(incidentId);
        
        var requiredServices = RequiredServiceTypesFor(incident.Type, incident.NumberOfInjured);
        var createdDeployments = new List<Deployment>();

        foreach (var serviceType in requiredServices)
        {
            var candidateTeams = await _responseTeamService.GetAvailableByServiceTypeAsync(serviceType);
            if (candidateTeams.Count == 0)
                continue;

            var ranked = candidateTeams.OrderBy(t => DistanceKm(
                    incident.Location.Latitude, incident.Location.Longitude,
                    t.BaseLocation.Latitude, t.BaseLocation.Longitude))
                .ToList();

            ResponseTeam? chosenTeam = null;
            Vehicle? chosenVehicle = null;

            foreach (var team in ranked)
            {
                var availableVehicles = await _vehicleService.GetAvailableForTeamAsync(team.Id);
                if (availableVehicles.Count == 0)
                    continue;

                chosenTeam = team;
                chosenVehicle = availableVehicles.FirstOrDefault();
                break;
            }
            
            if (chosenTeam is null || chosenVehicle is null)
                continue;
            
            var deployment = await _deploymentService.InsertAsync(incident.Id, chosenTeam.Id, chosenVehicle.Id,
                $"Autoassigned for {serviceType.ToString()}.");
            
            createdDeployments.Add(deployment);

            await _emailQueue.EnqueueAsync(new EmailMessage
            {
                To = chosenTeam.EmergencyService.ContactEmail,
                Subject = $"[ResQ] Team dispatched - {incident.Type} at {incident.Location?.Address}",
                HtmlBody =
                    $"Team '{chosenTeam.Name}' with vehicle '{chosenVehicle.PlateNumber}' was dispatched to incident " +
                    $"{incident.Id} (priority {incident.Priority}) at {incident.Location?.Address} - {incident.Location?.City}."
            });
        }
        
        return createdDeployments;
    }
     
     
    protected HashSet<ServiceType> RequiredServiceTypesFor(IncidentType type, int numberOfInjured)
    {
        var services = new HashSet<ServiceType>();

        if (type == IncidentType.Fire)
        {
            services.Add(ServiceType.FireDepartment);
        } else if (type == IncidentType.TrafficAccident)
        {
            services.Add(ServiceType.Police);
            services.Add(ServiceType.MedicalService);
        } else if (type == IncidentType.Medical)
        {
            services.Add(ServiceType.MedicalService);
        } else if (type == IncidentType.Flood)
        {
            services.Add(ServiceType.FireDepartment);
        }else
        {
            services.Add(ServiceType.Other);
        }
    
        return services;
    }

    public double? DistanceKm(double? lat1, double? lon1, double? lat2, double? lon2)
    {
        if (!lat1.HasValue || !lon1.HasValue || !lat2.HasValue || !lon2.HasValue)
        {
            return null;
        }
        const double earthRadiusKm = 6371.0;
    
        double l1 = lat1.Value;
        double l2 = lat2.Value;

        double dLat = DegreesToRadians(l2 - l1);
        double dLon = DegreesToRadians(lon2.Value - lon1.Value);

        double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                   Math.Cos(DegreesToRadians(l1)) * Math.Cos(DegreesToRadians(l2)) *
                   Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        return earthRadiusKm * c;
    }
    
    protected static double DegreesToRadians(double degrees) => degrees * (Math.PI / 180.0);

}