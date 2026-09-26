using Domain.Models;
using Repository.Interface;
using Service.Interface;

namespace Service.Implementation;

public class LocationService : ILocationService
{
    private readonly IRepository<Location> _repository;
    
    public LocationService(IRepository<Location> repository)
    {
        _repository = repository;
    }
    
    public async Task<List<Location>> GetAllAsync()
    {
        var result =  await _repository.GetAllAsync(
            selector: x => x
        );
        return result.ToList();
    }

    public async Task<Location?> GetByIdAsync(Guid id)
    {
        return await _repository.GetAsync(
            selector: x => x,
            predicate: x => x.Id == id);
    }

    public async Task<Location> GetByIdNotNullAsync(Guid id)
    {
        var result = await GetByIdAsync(id);

        if (result == null)
        {
            throw new InvalidOperationException($"Location with id {id} not found");
        }
        
        return result;
    }

    public async Task<Location> InsertAsync(String address, String city, double latitude, double longitude)
    {
        var location = new Location()
        {
            Address = address,
            City = city,
            Latitude = latitude,
            Longitude = longitude
        };
        
        return await _repository.InsertAsync(location);
    }

    public async Task<Location> UpdateAsync(Guid id, String address, String city, double latitude, double longitude)
    {
        var location = await GetByIdNotNullAsync(id);
        
        location.Address = address;
        location.City = city;
        location.Latitude = latitude;
        location.Longitude = longitude;
        
        return await _repository.UpdateAsync(location);
    }

    public async Task<Location> DeleteByIdAsync(Guid id)
    {
        var result = await GetByIdNotNullAsync(id);
        return await _repository.DeleteAsync(result);
    }
}