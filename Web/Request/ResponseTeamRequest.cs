using System;

namespace Web.Request;

public record ResponseTeamRequest(
    string Name,
    int NumberOfMembers,
    string Status,
    Guid EmergencyServiceId,
    Guid BaseLocationId
    );