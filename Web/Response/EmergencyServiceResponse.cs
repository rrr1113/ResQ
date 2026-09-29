using Domain.Enums;

namespace Web.Response;

public record EmergencyServiceResponse(
    String Name,
    ServiceType ServiceType,
    String ContactPhone,
    String ContactEmail,
    List<ResponseTeamBasicResponse> ResponseTeams
    );

public record EmergencyServiceBasicResponse(
    String Name,
    ServiceType ServiceType,
    String ContactPhone,
    String ContactEmail
    );