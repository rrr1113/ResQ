using System;
using System.Collections.Generic;
using Domain.Enums;

namespace Web.Response;

public record IncidentResponse(
    IncidentType Type, 
    DateTime ReportedAt,
    String Address,
    String City,
    String? Priority,
    String Status,
    List<ResponseTeamBasicResponse> ResponseTeams
    );

public record IncidentBasicResponse(
    String Type, 
    DateTime ReportedAt,
    String Address,
    String City
    );