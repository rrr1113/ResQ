using Domain.Common;
using Domain.Models;

namespace Domain.Dto;

public class WeatherSnapshotDto : BaseEntity
{
    public Guid LocationId { get; set; }
    public Location Location { get; set; } = null!;

    public double Temperature { get; set; } 
    public double WindSpeed { get; set; }
    public double Rain { get; set; } 
    public int WeatherCode { get; set; }
    public DateTime FetchedAt { get; set; } = DateTime.UtcNow;

    public bool IsSevere {get; set;}
}