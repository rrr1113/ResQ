using Domain.Dto;
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
        var result =  await _repository.GetAllAsync(selector: x => x);
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
        await _responseTeamService.GetByIdNotNullAsync(responseTeamId);
        
        var vehicle = new Vehicle()
        {
            PlateNumber = plateNumber,
            VehicleType = vehicleType,
            ResponseTeamId = responseTeamId
        };
        
        return await _repository.InsertAsync(vehicle);
    }

    public async Task<Vehicle> UpdateAsync(UpdateVehicleDto updateVehicleDto)
    {
        var vehicle = await GetByIdNotNullAsync(updateVehicleDto.Id);
        await _responseTeamService.GetByIdNotNullAsync(updateVehicleDto.ResponseTeamId);
        
        vehicle.PlateNumber = updateVehicleDto.PlateNumber;
        vehicle.VehicleType = updateVehicleDto.VehicleType;
        vehicle.Status = updateVehicleDto.Status;
        vehicle.ResponseTeamId = updateVehicleDto.ResponseTeamId;
        
        return await _repository.UpdateAsync(vehicle);
    }
    
    public async Task<Vehicle> DeleteByIdAsync(Guid id)
    {
        var result = await GetByIdNotNullAsync(id);
        return await _repository.DeleteAsync(result);
    }

    public async Task<List<Vehicle>> GetAvailableForTeamAsync(Guid teamId)
    {
        var result = await _repository.GetAllAsync(
            selector: x=>x,
            predicate: x=>x.ResponseTeamId == teamId && x.Status ==  VehicleStatus.Available,
            orderBy: x =>x.OrderBy(t => t.Capacity
            )
        );
        
        return result.ToList();
    }

    public async Task UpdateStatus(Guid vehicleId, VehicleStatus status)
    {
        var vehicle = await GetByIdNotNullAsync(vehicleId);
        vehicle.Status = status;
        await _repository.UpdateAsync(vehicle);
    }
}