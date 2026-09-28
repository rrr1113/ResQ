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
    private readonly HelperMethods _helperMethods;
    
    public IncidentService(IRepository<Incident> repository, ILocationService locationService, 
        IOperatorService operatorService, HelperMethods helperMethods)
    {
        _repository = repository;
        _locationService = locationService;
        _operatorService = operatorService;
        _helperMethods = helperMethods;
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
        PriorityLevel priority = await _helperMethods.CalculatePriorityAsync(incident);
        
        incident.Priority = priority;
        return await  _repository.UpdateAsync(incident);
    }

    public async Task<List<Incident>> GetAllByCity(string city)
    {
        var result = await _repository.GetAllAsync(
            selector: x=>x,
            predicate: x=>x.Location.City == city,
            orderBy: x => x.OrderBy(i => i.Priority)
        );

        return result.ToList();
    }

    public async Task UpdateStatus(Guid id, IncidentStatus status)
    {
        var incident = await GetByIdNotNullAsync(id);
        incident.Status = status;
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