using Domain.Dto;
using Domain.Models;

namespace Service.Interface;

public interface IDeploymentService
{
    Task<List<Deployment>> GetAllAsync(Guid? incidentId, Guid? teamId);
    Task<Deployment?> GetByIdAsync(Guid id);
    Task<Deployment> GetByIdNotNullAsync(Guid id);

    Task<Deployment> InsertAsync(Guid incidentId, Guid responseTeamId, Guid vehicleId, string? notes); 

    Task<Deployment> UpdateAsync(Guid id); 

    Task<Deployment> DeleteByIdAsync(Guid id);
    
    Task<PaginatedResult<Deployment>> GetPagedAsync(int pageNumber, int pageSize);

    Task<List<Deployment>> AssignTeamsForIncidentAsync(Guid incident);
}