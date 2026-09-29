using Domain.Dto;
using Domain.Enums;
using Domain.Models;

namespace Service.Interface;

public interface IIncidentService
{
    Task<List<Incident>> GetAllAsync(string? city, string? country);
    Task<Incident?> GetByIdAsync(Guid id);
    Task<Incident> GetByIdNotNullAsync(Guid id);

    Task<Incident> InsertAsync(IncidentType type, String description, int numberOfInjured, Guid locationId); 

    Task<Incident> UpdateAsync(Guid id, IncidentDto incidentDto); 

    Task<Incident> DeleteByIdAsync(Guid id);

    Task<List<Incident>> GetAllByCity(string city);
    
    Task UpdateStatus (Guid id, IncidentStatus status);

    Task<PaginatedResult<Incident>> GetPagedAsync(int pageNumber, int pageSize);

}