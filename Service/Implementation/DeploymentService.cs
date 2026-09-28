using Domain.Dto;
using Domain.Dto.Email;
using Domain.Enums;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using Service.Interface;

namespace Service.Implementation;

public class DeploymentService : IDeploymentService
{
    private readonly IRepository<Deployment> _repository;
    private readonly IResponseTeamService _responseTeamService;
    private readonly IVehicleService _vehicleService;
    private readonly IEmailQueue _emailQueue;
    private readonly HelperMethods _helperMethods;
    private readonly IIncidentService _incidentService;
    
    public DeploymentService(IRepository<Deployment> repository, 
        IResponseTeamService responseTeamService,
        IVehicleService vehicleService, IEmailQueue emailQueue, HelperMethods helperMethods,
        IIncidentService incidentService)
    {
        _repository = repository;
        _responseTeamService = responseTeamService;
        _vehicleService = vehicleService;
        _emailQueue = emailQueue;
        _helperMethods = helperMethods;
        _incidentService = incidentService;
    }
    
    public async Task<List<Deployment>> GetAllAsync(Guid? incidentId, Guid? teamId)
    {
        var result =  await _repository.GetAllAsync(
            selector: x => x,
            predicate: x => (incidentId == null || x.IncidentId == incidentId) 
                            && (teamId == null || x.ResponseTeamId == teamId)
        );
        return result.ToList();
    }

    public async Task<Deployment?> GetByIdAsync(Guid id)
    {
        return await _repository.GetAsync(
            selector: x => x,
            predicate: x => x.Id == id);
    }
    
    public async Task<Deployment> GetByIdNotNullAsync(Guid id)
    {
        var result = await GetByIdAsync(id);

        if (result == null)
        {
            throw new InvalidOperationException($"Deployment with id {id} not found");
        }
        
        return result;
    }

    public async Task<Deployment> InsertAsync(Guid incidentId, Guid responseTeamId, Guid vehicleId, string? notes)
    {
        var deployment = new Deployment()
        {
            IncidentId = incidentId,
            ResponseTeamId = responseTeamId,
            VehicleId = vehicleId,
            DispatchTime = DateTime.UtcNow,
            Notes = notes
        };
        
        var result = await _repository.InsertAsync(deployment);
        
        await _responseTeamService.UpdateStatus(responseTeamId, TeamStatus.Dispatched);
        await _vehicleService.UpdateStatus(vehicleId, VehicleStatus.InUse);

        return result;
    }

    public async Task<Deployment> UpdateAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<Deployment> DeleteByIdAsync(Guid id)
    {
        var result = await GetByIdNotNullAsync(id);
        return await _repository.DeleteAsync(result);
    }

    public async Task<PaginatedResult<Deployment>> GetPagedAsync(int pageNumber, int pageSize)
    {
        return await _repository.GetAllPagedAsync(
            selector: x => x,
            include: x=> x.Include(i => i.Incident),
            pageNumber: pageNumber,
            pageSize: pageSize,
            asNoTracking: true);
    }

    public async Task<List<Deployment>> AssignTeamsForIncidentAsync(Guid incidentId)
    {
        Incident incident = await _incidentService.GetByIdNotNullAsync(incidentId);
        
        var requiredServices = _helperMethods.RequiredServiceTypesFor(incident.Type, incident.NumberOfInjured);
        var created = new List<Deployment>();

        foreach (var serviceType in requiredServices)
        {
            var candidateTeams = await _responseTeamService.GetAvailableByServiceTypeAsync(serviceType);
            if (candidateTeams.Count == 0)
                continue;

            var ranked = candidateTeams.OrderBy(t => _helperMethods.DistanceKm(
                    incident.Location.Latitude, incident.Location.Longitude,
                    t.BaseLocation.Latitude, t.BaseLocation.Longitude))
                .ToList();

            ResponseTeam? chosenTeam = ranked.FirstOrDefault();

            var availableVehicles = await _vehicleService.GetAvailableForTeamAsync(chosenTeam.Id);
            Vehicle? chosenVehicle = availableVehicles.FirstOrDefault();
            ;

            if (chosenVehicle is null)
                continue;

            var deployment = await InsertAsync(incident.Id, chosenTeam.Id, chosenVehicle.Id, $"Autoassigned for {serviceType.ToString()}.");
            
            created.Add(deployment);

            await _emailQueue.EnqueueAsync(new EmailMessage
            {
                Subject = $"[ResQ] Team dispatched - {incident.Type} at {incident.Location?.Address}",
                To = chosenTeam.EmergencyService.ContactEmail,
                HtmlBody =
                    $"Team '{chosenTeam.Name}' with vehicle '{chosenVehicle.PlateNumber}' was dispatched to incident " +
                    $"{incident.Id} (priority {incident.Priority}) at {incident.Location?.Address} - {incident.Location?.City}."
            });
        }

        if (created.Count > 0)
        {
            incident.Status = IncidentStatus.Assigned;
        } // to do dali da pravam lista za unasigned incidenti da se procesiraat so background job retry na metodovvvv
        // dali autoassign posle samo kreiranje na incident ili da ima kopce za autoassign? podobro kopce?
        return created;
    }
}

