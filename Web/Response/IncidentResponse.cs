namespace Web.Response;

public record IncidentResponse(
    Guid Id,
    String Type, 
    DateTime ReportedAt,
    String Address,
    String City,
    String Country,
    String? Priority,
    String Status,
    double Temperature,
    double Rain,
    int WeatherCode,
    List<ResponseTeamBasicResponse> ResponseTeams
    );

public record IncidentBasicResponse(
    String Type, 
    DateTime ReportedAt,
    String Address,
    String City
    );