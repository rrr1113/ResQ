using Domain.Dto;
using Domain.Enums;
using Domain.Models;
using Web.Request;
using Web.Response;

namespace Web.Extensions;

public static class ResponseTeamExtensions
{
    public static ResponseTeamResponse ToResponse(this ResponseTeam responseTeam)
    {
        return new ResponseTeamResponse(
            responseTeam.Id.ToString(),
            responseTeam.Name,
            responseTeam.NumberOfMembers,
            responseTeam.Status.ToString(),
            responseTeam.BaseLocation.Address,
            responseTeam.BaseLocation.City,
            responseTeam.BaseLocation.Country,
            responseTeam.EmergencyService.ServiceType.ToString()
        );
    }
    
    public static List<ResponseTeamResponse> ToResponse(this List<ResponseTeam> responseTeams)
    {
        return responseTeams.Select(x => x.ToResponse()).ToList();
    }
    
    public static ResponseTeamBasicResponse ToBasicResponse(this ResponseTeam responseTeam)
    {
        return new ResponseTeamBasicResponse(
            responseTeam.Name,
            responseTeam.NumberOfMembers,
            responseTeam.Status.ToString()
        );
    }
    
    public static ResponseTeamDto ToDto(this ResponseTeamRequest team)
    {
        return new ResponseTeamDto
        {
            BaseLocationId = team.BaseLocationId,
            EmergencyServiceId = team.EmergencyServiceId,
            Name = team.Name,
            NumberOfMembers = team.NumberOfMembers,
            Status = Enum.Parse<TeamStatus>(team.Status)
        };
    }
}