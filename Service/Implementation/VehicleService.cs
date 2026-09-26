using Domain.Enums;
using Domain.Models;
using Repository.Interface;
using Service.Interface;

namespace Service.Implementation;

public class VehicleService : IVehicleService
{
    private readonly IRepository<Vehicle> _repository;
    private readonly IResponseTeamService _responseTeamService;
    
    public VehicleService(IRepository<Vehicle> repository, IResponseTeamService responseTeamService)
    {
        _repository = repository;
        _responseTeamService = responseTeamService;
    }
    
    public async Task<List<Vehicle>> GetAllAsync()
    {
        var result =  await _repository.GetAllAsync(
            selector: x => x
        );
        return result.ToList();
    }

    public async Task<Vehicle?> GetByIdAsync(Guid id)
    {
        return await _repository.GetAsync(
            selector: x => x,
            predicate: x => x.Id == id);
    }

    public async Task<Vehicle> GetByIdNotNullAsync(Guid id)
    {
        var result = await GetByIdAsync(id);

        if (result == null)
        {
            throw new InvalidOperationException($"Vehicle with id {id} not found");
        }
        
        return result;
    }

    public async Task<Vehicle> InsertAsync(string plateNumber, VehicleType vehicleType, Guid responseTeamId)
    {
        if (await _responseTeamService.GetByIdAsync(responseTeamId) == null)
        {
            throw new InvalidOperationException($"ResponseTeam with id {responseTeamId} not found");
        }
        
        var vehicle = new Vehicle()
        {
            PlateNumber = plateNumber,
            VehicleType = vehicleType,
            ResponseTeamId = responseTeamId
        };
        
        return await _repository.InsertAsync(vehicle);
    }

    public async Task<Vehicle> UpdateAsync(Guid id, string plateNumber, VehicleType vehicleType, VehicleStatus status, Guid responseTeamId)
    {
        var vehicle = await GetByIdNotNullAsync(id);
     
        if (await _responseTeamService.GetByIdAsync(responseTeamId) == null)
        {
            throw new InvalidOperationException($"ResponseTeam with id {responseTeamId} not found");
        }
        
        vehicle.PlateNumber = plateNumber;
        vehicle.VehicleType = vehicleType;
        vehicle.Status = status;
        vehicle.ResponseTeamId = responseTeamId;
        
        return await _repository.UpdateAsync(vehicle);
    }
    
    public async Task<Vehicle> DeleteByIdAsync(Guid id)
    {
        var result = await GetByIdNotNullAsync(id);
        return await _repository.DeleteAsync(result);
    }
}