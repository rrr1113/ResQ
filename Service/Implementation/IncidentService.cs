using Domain.Enums;
using Domain.Models;
using Repository.Interface;
using Service.Interface;

namespace Service.Implementation;

public class IncidentService : IIncidentService
{
    private readonly IRepository<Incident> _repository;
    private readonly ILocationService _locationService;
    private readonly IOperatorService _operatorService;
    private readonly IPriorityCalculationService _priorityCalculationService;
    
    public IncidentService(IRepository<Incident> repository, ILocationService locationService, 
        IOperatorService operatorService, IPriorityCalculationService priorityCalculationService)
    {
        _repository = repository;
        _locationService = locationService;
        _operatorService = operatorService;
        _priorityCalculationService = priorityCalculationService;
    }
    
    public async Task<List<Incident>> GetAllAsync()
    {
        var result =  await _repository.GetAllAsync(
            selector: x => x
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
        if (await _locationService.GetByIdAsync(locationId) == null)
        {
            throw new InvalidOperationException($"Location with id {locationId} not found");
        }
        
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
        
        return await _repository.InsertAsync(incident);
    }

    public async Task<Incident> UpdateAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public async Task<Incident> DeleteByIdAsync(Guid id)
    {
        var result = await GetByIdNotNullAsync(id);
        return await _repository.DeleteAsync(result);
    }

    public async Task<Incident> SetIncidentPriorityLevel(Guid id)
    {
        Incident incident = await GetByIdNotNullAsync(id);
        PriorityLevel priority = await _priorityCalculationService.CalculatePriorityAsync(incident);
        
        incident.Priority = priority;
        return await  _repository.UpdateAsync(incident);
    }
}