namespace Web.Response;

public record VehicleResponse(
    Guid Id,
    String PlateNumber,
    String VehicleType,
    String ResponseTeamName,
    String Status,
    int Capacity
    );
    
