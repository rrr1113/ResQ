using Domain.Models;
using Web.Response;

namespace Web.Extensions;

public static class ResponseTeamExtensions
{
    public static ResponseTeamResponse ToResponse(this ResponseTeam responseTeam)
    {
        return new ResponseTeamResponse(
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
}