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
    private readonly IIncidentService _incidentService;
    private readonly IEmailQueue _emailQueue;
    
    public DeploymentService(IRepository<Deployment> repository, 
        IResponseTeamService responseTeamService,
        IVehicleService vehicleService, IIncidentService incidentService,
        IEmailQueue emailQueue)
    {
        _repository = repository;
        _responseTeamService = responseTeamService;
        _vehicleService = vehicleService;
        _incidentService = incidentService;
        _emailQueue = emailQueue;

    }
    
    public async Task<List<Deployment>> GetAllAsync(Guid? incidentId, Guid? teamId)
    {
        var result =  await _repository.GetAllAsync(
            selector: x => x,
            predicate: x => (incidentId == null || x.IncidentId == incidentId) 
                            && (teamId == null || x.ResponseTeamId == teamId),
            include: x => x.Include(i => i.Incident)
                .Include(i => i.Vehicle)
                .Include(i => i.ResponseTeam)
        );
        return result.ToList();
    }

    public async Task<Deployment?> GetByIdAsync(Guid id)
    {
        return await _repository.GetAsync(
            selector: x => x,
            predicate: x => x.Id == id,
            include: x => x.Include(i => i.Incident)
                .Include(i => i.Vehicle)
                .Include(i => i.ResponseTeam));
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
        var incident = await _incidentService.GetByIdNotNullAsync(incidentId);
        var team = await _responseTeamService.GetByIdNotNullAsync(responseTeamId);
        var vehicle = await _vehicleService.GetByIdNotNullAsync(vehicleId);

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
        await _incidentService.UpdateStatus(incidentId, IncidentStatus.Assigned);
        
        await _emailQueue.EnqueueAsync(new EmailMessage
        {
            To = team.EmergencyService.ContactEmail,
            Subject = $"[ResQ] Team dispatched - {incident.Type} at {incident.Location?.Address}",
            HtmlBody =
                $"Team '{team.Name}' with vehicle '{vehicle.PlateNumber}' was dispatched to incident " +
                $"{incident.Id} (priority {incident.Priority}) at {incident.Location?.Address} - {incident.Location?.City}."
        });
        
        return result;
    }

    public async Task<Deployment> UpdateAsync(Guid id, DeploymentDto updateDeploymentDto)
    {
        var deployment = await GetByIdNotNullAsync(id);
        
        deployment.DispatchTime = updateDeploymentDto.DispatchTime;
        deployment.ArrivalTime = updateDeploymentDto.ArrivalTime;
        deployment.CompletionTime = updateDeploymentDto.CompletionTime;
        deployment.Notes =  updateDeploymentDto.Notes;
        deployment.IncidentId = updateDeploymentDto.IncidentId;
        deployment.ResponseTeamId = updateDeploymentDto.ResponseTeamId;
        deployment.VehicleId = updateDeploymentDto.VehicleId;
        
        return await _repository.UpdateAsync(deployment);
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
}

