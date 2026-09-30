namespace Web.Request;

public record VehicleRequest(
    String PlateNumber,
    String VehicleType,
    Guid ResponseTeamId
    );