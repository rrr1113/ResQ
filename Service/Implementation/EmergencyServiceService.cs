using Domain.Enums;
using Domain.Models;
using Repository.Interface;
using Service.Interface;

namespace Service.Implementation;

public class EmergencyServiceService : IEmergencyServiceService
{
    private readonly IRepository<EmergencyService> _repository;
    
    public EmergencyServiceService(IRepository<EmergencyService> repository)
    {
        _repository = repository;
    }
    
    public async Task<List<EmergencyService>> GetAllAsync()
    {
        var result =  await _repository.GetAllAsync(
            selector: x => x
        );
        return result.ToList();
    }

    public async Task<EmergencyService?> GetByIdAsync(Guid id)
    {
        return await _repository.GetAsync(
            selector: x => x,
            predicate: x => x.Id == id);
    }

    public async Task<EmergencyService> GetByIdNotNullAsync(Guid id)
    {
        var result = await GetByIdAsync(id);

        if (result == null)
        {
            throw new InvalidOperationException($"Emergency Service with id {id} not found");
        }
        
        return result;
    }

    public async Task<EmergencyService> InsertAsync(string name, ServiceType serviceType, string contactPhone, string contactEmail)
    {
        var emergencyService = new EmergencyService()
        {
            Name = name,
            ServiceType = serviceType,
            ContactPhone = contactPhone,
            ContactEmail = contactEmail,
        };

        return await _repository.InsertAsync(emergencyService);
    }

    public async Task<EmergencyService> UpdateAsync(Guid id, string name, ServiceType serviceType, string contactPhone, string contactEmail)
    {
        var emergencyService = await GetByIdNotNullAsync(id);
        
        emergencyService.Name = name;
        emergencyService.ServiceType = serviceType;
        emergencyService.ContactPhone = contactPhone;
        emergencyService.ContactEmail = contactEmail;
        
        return await _repository.UpdateAsync(emergencyService);
    }

    public async Task<EmergencyService> DeleteByIdAsync(Guid id)
    {
        var result = await GetByIdNotNullAsync(id);
        return await _repository.DeleteAsync(result);
    }

    public async Task<List<EmergencyService>> GetByName(string name)
    {
        var result = await _repository.GetAllAsync(
            selector: x=>x,
            predicate: x=>x.Name.Contains(name)
        );

        return result.ToList();
    }
}