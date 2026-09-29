using System;

namespace Web.Request;

public record DeploymentRequest(
    Guid IncidentId,
    Guid ResponseTeamId,
    Guid VehicleId,
    String? Notes
    );
    
    public record DeploymentUpdateRequest(
        DateTime ArrivalTime,
        DateTime CompletionTime,
        DateTime DispatchTime,
        Guid IncidentId,
        Guid ResponseTeamId,
        Guid VehicleId,
        String? Notes
        );