using Domain.Enums;
using Domain.Models;
using Service.Interface;

namespace Service.Implementation;

public class PriorityCalculationService : IPriorityCalculationService
{
    private readonly IWeatherSnapshotService _weatherSnapshotService;

    public PriorityCalculationService(IWeatherSnapshotService weatherSnapshotService)
    {
        _weatherSnapshotService = weatherSnapshotService;
    }
    
    private static int BaseScoreForType(IncidentType type) 
    {
        if (type == IncidentType.Fire ||  type == IncidentType.Flood || type == IncidentType.Earthquake)
        { 
            return 3;
        } 
        if(type == IncidentType.TrafficAccident  || type == IncidentType.Medical || type == IncidentType.Crime)
        {
            return 2;
        } 
        return 1;
    }
    
    public async Task<PriorityLevel> CalculatePriorityAsync(Incident incident)
    {
        var score = BaseScoreForType(incident.Type);

        if (incident.NumberOfInjured >= 1 && incident.NumberOfInjured <= 5)
        {
            score += 1;
        } else if (incident.NumberOfInjured > 5 && incident.NumberOfInjured <= 10)
        {
            score += 2;
        } else if (incident.NumberOfInjured > 10)
        {
            score += 3;
        }
        
        var weather = await _weatherSnapshotService.GetWeatherDataForLocationIdAsync(incident.LocationId);
        if (weather.IsSevere)
            score += 3;

        return ScoreToPriority(score);
    }

    
    private static PriorityLevel ScoreToPriority(int score) 
    {
        if (score >= 3 && score <= 4)
        {
            return PriorityLevel.Medium;
        } 
        if (score >= 5)
        {
            return PriorityLevel.Critical;
        }
        return PriorityLevel.Low;
    }
}