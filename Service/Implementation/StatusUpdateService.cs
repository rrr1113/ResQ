using Domain.Enums;
using Domain.Models;
using Repository.Interface;
using Service.Interface;

namespace Service.Implementation;

public class StatusUpdateService : IStatusUpdateService
{
    private readonly IRepository<StatusUpdate> _repository;
    private readonly IOperatorService _operatorService;

    
    public StatusUpdateService(IRepository<StatusUpdate> repository, IOperatorService operatorService)
    {
        _repository = repository;
        _operatorService = operatorService;
    }
    
    public async Task<List<StatusUpdate>> GetAllAsync()
    {
        var result =  await _repository.GetAllAsync(
            selector: x => x
        );
        return result.ToList();
    }

    public async Task<StatusUpdate?> GetByIdAsync(Guid id)
    {
        return await _repository.GetAsync(
            selector: x => x,
            predicate: x => x.Id == id);
    }

    public async Task<StatusUpdate> GetByIdNotNullAsync(Guid id)
    {
        var result = await GetByIdAsync(id);

        if (result == null)
        {
            throw new InvalidOperationException($"Status Update with id {id} not found");
        }
        
        return result;
    }

    public async Task<StatusUpdate> InsertAsync(Guid incidentId, string? note, IncidentStatus incidentStatus)
    {
        var statusUpdate = new StatusUpdate()
        {
            IncidentId = incidentId,
            Note = note,
            Status = incidentStatus,
            Timestamp = DateTime.UtcNow,
            UpdatedByOperatorId = _operatorService.GetUserId()
        };
        
        return await _repository.InsertAsync(statusUpdate);
    }

    public async Task<StatusUpdate> UpdateAsync(Guid id, Guid incidentId, string? note, IncidentStatus incidentStatus)
    {
        var statusUpdate = await GetByIdNotNullAsync(id);
        
        statusUpdate.IncidentId = incidentId;
        statusUpdate.Note = note;
        statusUpdate.Status = incidentStatus;
        
        return await _repository.UpdateAsync(statusUpdate);
    }

    public async Task<StatusUpdate> DeleteByIdAsync(Guid id)
    {
        var result = await GetByIdNotNullAsync(id);
        return await _repository.DeleteAsync(result);
    }
}