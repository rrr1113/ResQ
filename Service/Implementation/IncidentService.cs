using Domain.Dto;
using Domain.Enums;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using Service.Interface;

namespace Service.Implementation;

public class IncidentService : IIncidentService
{
    private readonly IRepository<Incident> _repository;
    private readonly ILocationService _locationService;
    private readonly IOperatorService _operatorService;
    private readonly PriorityCalculatorService _priorityCalculatorService;
    
    public IncidentService(IRepository<Incident> repository, ILocationService locationService, 
        IOperatorService operatorService, PriorityCalculatorService priorityCalculatorService)
    {
        _repository = repository;
        _locationService = locationService;
        _operatorService = operatorService;
        _priorityCalculatorService = priorityCalculatorService;
    }
    
    public async Task<List<Incident>> GetAllAsync(string? city, string? country)
    {
        var result =  await _repository.GetAllAsync(
            selector: x => x,
            predicate: x => (city == null || x.Location.City.Contains(city)) 
                      && (country == null || x.Location.Country.Contains(country))
        );
        return result.ToList();
    }

    public async Task<Incident?> GetByIdAsync(Guid id)
    {
        return await _repository.GetAsync(
            selector: x => x,
            predicate: x => x.Id == id);
    }

    public async Task<Incident> GetByIdNotNullAsync(Guid id)
    {
        var result = await GetByIdAsync(id);

        if (result == null)
        {
            throw new InvalidOperationException($"Incident with id {id} not found");
        }
        
        return result;
    }

    public async Task<Incident> InsertAsync(IncidentType type, string description, int numberOfInjured, Guid locationId)
    {
        await _locationService.GetByIdAsync(locationId);
        
        var incident = new Incident
        {
            Type = type,
            Description = description,
            NumberOfInjured = numberOfInjured,
            ReportedAt = DateTime.UtcNow,
            Status = IncidentStatus.Reported,
            LocationId = locationId,
            OperatorId = _operatorService.GetUserId()
        };
        
        incident = await _repository.InsertAsync(incident);
        
        incident = await GetByIdNotNullAsync(incident.Id);

        incident.Priority = await _priorityCalculatorService.CalculatePriorityAsync(incident);
        await _repository.UpdateAsync(incident);
        //await _teamAssignmentService.AssignTeamsForIncidentAsync(incident.Id);
        
        return incident;
    }

    public async Task<Incident> UpdateAsync(Guid id, IncidentDto incidentDto)
    {
        await _locationService.GetByIdAsync(incidentDto.LocationId);
        
        var incident = await GetByIdNotNullAsync(id);
        
        incident.Type = incidentDto.Type;
        incident.Description = incidentDto.Description;
        incident.NumberOfInjured = incidentDto.NumberOfInjured;
        incident.Priority = incidentDto.Priority;
        incident.Status = incidentDto.Status;
        incident.LastStatusUpdate = incidentDto.LastStatusUpdate;
        incident.LocationId = incidentDto.LocationId;
        
        return await _repository.UpdateAsync(incident);
    }

    public async Task<Incident> DeleteByIdAsync(Guid id)
    {
        var result = await GetByIdNotNullAsync(id);
        return await _repository.DeleteAsync(result);
    }

    public async Task UpdateStatus(Guid id, IncidentStatus status)
    {
        var incident = await GetByIdNotNullAsync(id);
        incident.Status = status;
        incident.LastStatusUpdate = DateTime.UtcNow;
        await _repository.UpdateAsync(incident);
    }
    
    public async Task<PaginatedResult<Incident>> GetPagedAsync(int pageNumber, int pageSize)
    {
        return await _repository.GetAllPagedAsync(
            selector: x => x,
            include: x=> x.Include(i => i.Location),
            pageNumber: pageNumber,
            pageSize: pageSize,
            asNoTracking: true);
    }
}