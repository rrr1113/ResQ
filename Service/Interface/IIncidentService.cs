using Domain.Enums;
using Domain.Models;

namespace Service.Interface;

public interface IIncidentService
{
    Task<List<Incident>> GetAllAsync();
    Task<Incident?> GetByIdAsync(Guid id);
    Task<Incident> GetByIdNotNullAsync(Guid id);

    Task<Incident> InsertAsync(IncidentType type, String description, int numberOfInjured, Guid locationId); 

    Task<Incident> UpdateAsync(Guid id); //TODO

    Task<Incident> DeleteByIdAsync(Guid id);

    Task<List<Incident>> GetAllByCity(string city);
}