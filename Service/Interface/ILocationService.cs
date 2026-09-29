using Domain.Models;

namespace Service.Interface;

public interface ILocationService
{
    Task<List<Location>> GetAllAsync();
    Task<Location?> GetByIdAsync(Guid id);
    Task<Location> GetByIdNotNullAsync(Guid id);

    Task<Location> InsertAsync(String address, String city, String country, double? latitude, double? longitude); 

    Task<Location> UpdateAsync(Guid id, String address, String city, String country, double latitude, double longitude); 

    Task<Location> DeleteByIdAsync(Guid id);
    
    
    Task<List<Location>> GetByAddress(String address);
}