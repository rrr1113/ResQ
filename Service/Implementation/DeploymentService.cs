using Domain.Models;
using Repository.Interface;
using Service.Interface;

namespace Service.Implementation;

public class DeploymentService : IDeploymentService
{
    private readonly IRepository<Deployment> _repository;
    
    public DeploymentService(IRepository<Deployment> repository)
    {
        _repository = repository;
    }
    
    public async Task<List<Deployment>> GetAllAsync()
    {
        var result =  await _repository.GetAllAsync(
            selector: x => x
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
        throw new NotImplementedException();
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
}