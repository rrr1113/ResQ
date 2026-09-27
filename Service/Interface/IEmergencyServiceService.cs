using Domain.Enums;
using Domain.Models;

namespace Service.Interface;

public interface IEmergencyServiceService
{
    Task<List<EmergencyService>> GetAllAsync();
    Task<EmergencyService?> GetByIdAsync(Guid id);
    Task<EmergencyService> GetByIdNotNullAsync(Guid id);

    Task<EmergencyService> InsertAsync(string name, ServiceType serviceType, string contactPhone, string contactEmail); 

    Task<EmergencyService> UpdateAsync(Guid id, string name, ServiceType serviceType, string contactPhone, string contactEmail); 

    Task<EmergencyService> DeleteByIdAsync(Guid id);

    Task<List<EmergencyService>> GetByName(String name);
}