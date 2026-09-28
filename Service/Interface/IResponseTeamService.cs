using Domain.Enums;
using Domain.Models;

namespace Service.Interface;

public interface IResponseTeamService
{
    Task<List<ResponseTeam>> GetAllAsync();
    Task<ResponseTeam?> GetByIdAsync(Guid id);
    Task<ResponseTeam> GetByIdNotNullAsync(Guid id);

    Task<ResponseTeam> InsertAsync(String name, int numberOfMembers, TeamStatus status, Guid emergencyServiceId, Guid baseLocationId); 

    Task<ResponseTeam> UpdateAsync(Guid id, String name, int numberOfMembers, TeamStatus status, Guid emergencyServiceId, Guid baseLocationId); //TODO

    Task<ResponseTeam> DeleteByIdAsync(Guid id);
    
    Task<List<ResponseTeam>> GetByEmergencyServiceId(Guid emergencyServiceId);
    
    Task<List<ResponseTeam>> GetAvailableByServiceTypeAsync(ServiceType serviceType);
    
    Task UpdateStatus(Guid responseTeamId, TeamStatus status);

}