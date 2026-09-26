using Domain.Enums;
using Domain.Models;

namespace Service.Interface;

public interface IStatusUpdateService
{
    Task<List<StatusUpdate>> GetAllAsync();
    Task<StatusUpdate?> GetByIdAsync(Guid id);
    Task<StatusUpdate> GetByIdNotNullAsync(Guid id);

    Task<StatusUpdate> InsertAsync(Guid incidentId, String? note, IncidentStatus incidentStatus); 

    Task<StatusUpdate> UpdateAsync(Guid id, Guid incidentId, String? note, IncidentStatus incidentStatus); //TODO

    Task<StatusUpdate> DeleteByIdAsync(Guid id);
}