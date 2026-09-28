namespace Web.Response;

public record DeploymentResponse(
    DateTime DispatchTime,
    DateTime? ArrivalTime,
    DateTime? CompletionTime,
    string IncidentType,
    string Location,
    String ResponseTeamName,
    String VehiclePlate
    );

public record DeploymentBasicResponse(
    DateTime DispatchTime,
    DateTime? ArrivalTime,
    string Location,
    String ResponseTeamName,
    String VehiclePlate
    );