namespace Web.Request;

public record DeploymentRequest(
    Guid IncidentId,
    Guid ResponseTeamId,
    Guid VehicleId,
    String? Notes
    );