using Domain.Enums;
using Service.Interface;
using Web.Extensions;
using Web.Request;
using Web.Response;

namespace Web.Mappers;

public class ResponseTeamMapper
{
    private readonly IResponseTeamService _responseTeamService;

    public ResponseTeamMapper(IResponseTeamService responseTeamService)
    {
        _responseTeamService = responseTeamService;
    }
    
    public async Task<List<ResponseTeamResponse>> GetAllAsync()
    {
        var result = await _responseTeamService.GetAllAsync();
        return result.ToResponse();
    }
    
    public async Task<ResponseTeamResponse> GetAsync(Guid id)
    {
        var result = await _responseTeamService.GetByIdNotNullAsync(id);
        return result.ToResponse();
    }
    
    public async Task<ResponseTeamResponse> InsertAsync(ResponseTeamRequest request)
    {
        var result = await _responseTeamService.InsertAsync(request.Name, request.NumberOfMembers, request.EmergencyServiceId, request.BaseLocationId);
        return result.ToResponse();
    }
    
    public async Task DeleteAsync(Guid id)
    {
        await _responseTeamService.DeleteByIdAsync(id);
    }
    
    public async Task<ResponseTeamResponse> UpdateAsync(Guid id, ResponseTeamRequest request)
    {
        var responseTeamDto = request.ToDto();
        var result = await _responseTeamService.UpdateAsync(id, responseTeamDto);
        return result.ToResponse();
    }
    
    public async Task UpdateStatus(Guid teamId, TeamStatus status)
    {
        await _responseTeamService.UpdateStatus(teamId, status);
    }
}