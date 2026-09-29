using System;
using System.Collections.Generic;

namespace Web.Response;

public record EmergencyServiceResponse(
    String Id,
    String Name,
    String ServiceType,
    String ContactPhone,
    String ContactEmail,
    List<ResponseTeamBasicResponse> ResponseTeams
    );

public record EmergencyServiceBasicResponse(
    String Name,
    String ServiceType,
    String ContactPhone,
    String ContactEmail
    );