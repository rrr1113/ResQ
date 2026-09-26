using Domain.Common;

namespace Domain.Models;

public class WeatherSnapshot : BaseEntity
{
    public Guid LocationId { get; set; }
    public Location Location { get; set; } = null!;

    public double Temperature { get; set; } 
    public double WindSpeed { get; set; }
    public double Rain { get; set; } 
    public int WeatherCode { get; set; }
    public DateTime FetchedAt { get; set; } = DateTime.UtcNow;

    // TODO dali vo service
    public bool IsSevere => WindSpeed > 50 || Rain > 10 || WeatherCode >= 95;
}