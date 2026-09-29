using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Dto;
using Domain.Enums;
using Domain.Models;
using Web.Request;
using Web.Response;

namespace Web.Extensions;

public static class IncidentExtensions
{
    public static IncidentResponse ToResponse(this Incident incident)
    {
        return new IncidentResponse(
            incident.Type,
            incident.ReportedAt,
            incident.Location.Address,
            incident.Location.City,
            incident.Priority?.ToString(),
            incident.Status.ToString(),
            incident.Deployments.Select(d => d.ResponseTeam.ToBasicResponse()).ToList()
        );
    }
    
    public static List<IncidentResponse> ToResponse(this List<Incident> incidents)
    {
        return incidents.Select(x => x.ToResponse()).ToList();
    }
    
    public static IncidentBasicResponse ToBasicResponse(this Incident incident)
    {
        return new IncidentBasicResponse(
            incident.Type.ToString(),
            incident.ReportedAt,
            incident.Location.Address,
            incident.Location.City
        );
    }
    
    public static IncidentDto ToDto(this IncidentUpdateRequest incident)
    {
        return new IncidentDto
        {
            Type = Enum.Parse<IncidentType>(incident.Type),
            Description = incident.Description,
            NumberOfInjured = incident.NumberOfInjured,
            Priority = Enum.Parse<PriorityLevel>(incident.Priority),
            Status = Enum.Parse<IncidentStatus>(incident.Status),
            LastStatusUpdate = incident.LastStatusUpdate,
            LocationId = incident.LocationId,
        };
    }
}