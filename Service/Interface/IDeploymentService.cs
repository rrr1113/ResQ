using Domain.Models;

namespace Service.Interface;

public interface IDeploymentService
{
    Task<List<Deployment>> GetAllAsync();
    Task<Deployment?> GetByIdAsync(Guid id);
    Task<Deployment> GetByIdNotNullAsync(Guid id);

    Task<Deployment> InsertAsync(Guid incidentId, Guid responseTeamId, Guid vehicleId, string? notes); //TODO

    Task<Deployment> UpdateAsync(Guid id); 

    Task<Deployment> DeleteByIdAsync(Guid id);
}