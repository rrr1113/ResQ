using Domain.Enums;
using Domain.Models;
using Service.Interface;

namespace Service.Implementation;

public class HelperMethods 
{
    private readonly IWeatherSnapshotService _weatherSnapshotService;

    public HelperMethods(IWeatherSnapshotService weatherSnapshotService)
    {
        _weatherSnapshotService = weatherSnapshotService;
    }
    
    //Priority calculation
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
    
    //Priority calculation
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

    //Priority calculation
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
    
    
    //Auto dispatch 
    public HashSet<ServiceType> RequiredServiceTypesFor(IncidentType type, int numberOfInjured)
    {
        var services = new HashSet<ServiceType>();

        if (type == IncidentType.Fire)
        {
            services.Add(ServiceType.FireDepartment);
        } else if (type == IncidentType.TrafficAccident)
        {
            services.Add(ServiceType.Police);
            services.Add(ServiceType.MedicalService);
        } else if (type == IncidentType.Medical)
        {
            services.Add(ServiceType.MedicalService);
        } else if (type == IncidentType.Flood)
        {
            services.Add(ServiceType.FireDepartment);
        }else
        {
            services.Add(ServiceType.Other);
        }
    
        return services;
    }

    //Auto dispatch 
    public double? DistanceKm(double? lat1, double? lon1, double? lat2, double? lon2)
    {
        if (!lat1.HasValue || !lon1.HasValue || !lat2.HasValue || !lon2.HasValue)
        {
            return null;
        }
        const double earthRadiusKm = 6371.0;
    
        double l1 = lat1.Value;
        double l2 = lat2.Value;

        double dLat = DegreesToRadians(l2 - l1);
        double dLon = DegreesToRadians(lon2.Value - lon1.Value);

        double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                   Math.Cos(DegreesToRadians(l1)) * Math.Cos(DegreesToRadians(l2)) *
                   Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        return earthRadiusKm * c;
    }
    
    //Auto dispatch 
    private static double DegreesToRadians(double degrees) => degrees * (Math.PI / 180.0);

}