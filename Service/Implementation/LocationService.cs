using Domain.Dto;
using Domain.Models;
using Repository.Interface;
using Service.Interface;

namespace Service.Implementation;

public class LocationService : ILocationService
{
    private readonly IRepository<Location> _repository;
    private readonly IGeocodingApiClient _geocodingClient;
    
    public LocationService(IRepository<Location> repository, IGeocodingApiClient geocodingClient)
    {
        _repository = repository;
        _geocodingClient = geocodingClient;
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

    public async Task<Location> InsertAsync(String address, String city, String country)
    {
        var existing = (await _repository.GetAllAsync(
            selector: l => l,
            predicate: l =>
                l.Address.ToLower() == address.ToLower() && 
                l.City.ToLower() == city.ToLower() && 
                l.Country.ToLower() == country.ToLower())).FirstOrDefault();

        if (existing is not null)
            return existing;

        var coordinates = await _geocodingClient.GetLongitudeAndLatitudeForAddress(address, city, country);

        var entity = new Location
        {
            Address = address,
            City = city,
            Country = country,
            Latitude = coordinates.Latitude,
            Longitude = coordinates.Longitude
        };
        
        return await _repository.InsertAsync(entity);
    }

    public async Task<Location> UpdateAsync(Guid id, String address, String city, String country, double latitude, double longitude)
    {
        var location = await GetByIdNotNullAsync(id);
        
        location.Address = address;
        location.City = city;
        location.Latitude = latitude;
        location.Longitude = longitude;
        location.Country = country;
        
        return await _repository.UpdateAsync(location);
    }

    public async Task<Location> DeleteByIdAsync(Guid id)
    {
        var result = await GetByIdNotNullAsync(id);
        return await _repository.DeleteAsync(result);
    }

    public async Task<List<Location>> GetByAddress(string address)
    {
        var result = await _repository.GetAllAsync(
            selector: x => x,
            predicate: x => x.Address.Contains(address)
        );
        
        return result.ToList();
    }
}