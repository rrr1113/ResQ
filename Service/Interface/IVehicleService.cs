using Domain.Dto;
using Domain.Enums;
using Domain.Models;

namespace Service.Interface;

public interface IVehicleService
{
    Task<List<Vehicle>> GetAllAsync();
    Task<Vehicle?> GetByIdAsync(Guid id);
    Task<Vehicle> GetByIdNotNullAsync(Guid id);

    Task<Vehicle> InsertAsync(String plateNumber, VehicleType vehicleType, Guid responseTeamId); 

    Task<Vehicle> UpdateAsync(UpdateVehicleDto updateVehicleDto); 

    Task<Vehicle> DeleteByIdAsync(Guid id);
    
    Task<List<Vehicle>> GetAvailableForTeamAsync(Guid teamId);
    
    Task UpdateStatus(Guid vehicleId, VehicleStatus status);
}