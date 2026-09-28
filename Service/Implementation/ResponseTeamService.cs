using Domain.Enums;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Repository.Interface;
using Service.Interface;

namespace Service.Implementation;

public class ResponseTeamService : IResponseTeamService
{
    private readonly IRepository<ResponseTeam> _repository;
    private readonly ILocationService _locationService;
    private readonly IEmergencyServiceService _emergencyServiceService;
    
    public ResponseTeamService(IRepository<ResponseTeam> repository, ILocationService locationService,  IEmergencyServiceService emergencyServiceService)
    {
        _repository = repository;
        _locationService = locationService;
        _emergencyServiceService = emergencyServiceService;
    }
    
    public async Task<List<ResponseTeam>> GetAllAsync()
    {
        var result =  await _repository.GetAllAsync(
            selector: x => x
        );
        return result.ToList();
    }

    public async Task<ResponseTeam?> GetByIdAsync(Guid id)
    {
        return await _repository.GetAsync(
            selector: x => x,
            predicate: x => x.Id == id);
    }

    public async Task<ResponseTeam> GetByIdNotNullAsync(Guid id)
    {
        var result = await GetByIdAsync(id);

        if (result == null)
        {
            throw new InvalidOperationException($"Response Team with id {id} not found");
        }
        
        return result;
    }

    public async Task<ResponseTeam> InsertAsync(string name, int numberOfMembers, TeamStatus status, Guid emergencyServiceId, Guid baseLocationId)
    {
        if (await _emergencyServiceService.GetByIdAsync(emergencyServiceId) == null)
        {
            throw new InvalidOperationException($"Emergency Service with id {emergencyServiceId} not found");
        }
        
        if (await _locationService.GetByIdAsync(baseLocationId) == null)
        {
            throw new InvalidOperationException($"Location with id {emergencyServiceId} not found");
        }
        
        var responseTeam = new ResponseTeam()
        {
            Name = name,
            NumberOfMembers = numberOfMembers,
            Status = status,
            EmergencyServiceId = emergencyServiceId,
            BaseLocationId = baseLocationId
        };
        
        return await _repository.InsertAsync(responseTeam);
    }

    public async Task<ResponseTeam> UpdateAsync(Guid id, string name, int numberOfMembers, TeamStatus status, Guid emergencyServiceId,
        Guid baseLocationId)
    {
        var responseTeam = await GetByIdNotNullAsync(id);
        
        if (await _emergencyServiceService.GetByIdAsync(emergencyServiceId) == null)
        {
            throw new InvalidOperationException($"Emergency Service with id {emergencyServiceId} not found");
        }
        
        if (await _locationService.GetByIdAsync(baseLocationId) == null)
        {
            throw new InvalidOperationException($"Location with id {emergencyServiceId} not found");
        }
        
        responseTeam.Name = name;
        responseTeam.NumberOfMembers = numberOfMembers;
        responseTeam.Status = status;
        responseTeam.EmergencyServiceId = emergencyServiceId;
        responseTeam.BaseLocationId = baseLocationId;
        
        return await _repository.UpdateAsync(responseTeam);
    }
    
    public async Task<ResponseTeam> DeleteByIdAsync(Guid id)
    {
        var result = await GetByIdNotNullAsync(id);
        return await _repository.DeleteAsync(result);
    }

    public async Task<List<ResponseTeam>> GetByEmergencyServiceId(Guid emergencyServiceId)
    {
        var result = await _repository.GetAllAsync(
            selector: x=>x,
            predicate: x=>x.EmergencyServiceId == emergencyServiceId,
            include: x=>x.Include(t => t.EmergencyService),
            orderBy: x =>x.OrderBy(t => t.NumberOfMembers
            )
        );
        
        return result.ToList();
    }

    public async Task<List<ResponseTeam>> GetAvailableByServiceTypeAsync(ServiceType serviceType)
    {
        var result = await _repository.GetAllAsync(
            selector: x=>x,
            predicate: x=>x.EmergencyService.ServiceType == serviceType,
            include: x=>x.Include(t => t.EmergencyService),
            orderBy: x =>x.OrderBy(t => t.NumberOfMembers
            )
        );
        
        return result.ToList();
    }

    public async Task UpdateStatus(Guid responseTeamId, TeamStatus status)
    {
        var team = await GetByIdNotNullAsync(responseTeamId);
        team.Status = status;
        await _repository.UpdateAsync(team);
    }
}