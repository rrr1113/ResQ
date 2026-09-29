using System.Collections.Generic;
using System.Linq;
using Domain.Models;
using Web.Response;

namespace Web.Extensions;

public static class EmergencyServiceExtensions
{
    public static EmergencyServiceResponse ToResponse(this EmergencyService emergencyService)
    {
        return new EmergencyServiceResponse(
            emergencyService.Id.ToString(),
            emergencyService.Name,
            emergencyService.ServiceType.ToString(),
            emergencyService.ContactPhone,
            emergencyService.ContactEmail,
            emergencyService.ResponseTeams.Select(x => x.ToBasicResponse()).ToList()
        );
    }
    
    public static List<EmergencyServiceResponse> ToResponse(this List<EmergencyService> emergencyServices)
    {
        return emergencyServices.Select(x => x.ToResponse()).ToList();
    }
    
    public static EmergencyServiceBasicResponse ToBasicResponse(this EmergencyService emergencyService)
    {
        return new EmergencyServiceBasicResponse(
            emergencyService.Name,
            emergencyService.ServiceType.ToString(),
            emergencyService.ContactPhone,
            emergencyService.ContactEmail
        );
    }
}