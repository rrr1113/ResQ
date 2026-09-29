using Domain.Models;
using Web.Response;

namespace Web.Extensions;

public static class EmergencyServiceExtensions
{
    public static EmergencyServiceResponse ToResponse(this EmergencyService emergencyService)
    {
        return new EmergencyServiceResponse(
            emergencyService.Name,
            emergencyService.ServiceType,
            emergencyService.ContactPhone,
            emergencyService.ContactEmail,
            emergencyService.ResponseTeams.Select(x => x.ToBasicResponse()).ToList()
        );
    }
    
    public static List<EmergencyServiceBasicResponse> ToResponse(this List<EmergencyService> emergencyServices)
    {
        return emergencyServices.Select(x => x.ToBasicResponse()).ToList();
    }
    
    public static EmergencyServiceBasicResponse ToBasicResponse(this EmergencyService emergencyService)
    {
        return new EmergencyServiceBasicResponse(
            emergencyService.Name,
            emergencyService.ServiceType,
            emergencyService.ContactPhone,
            emergencyService.ContactEmail
        );
    }
}