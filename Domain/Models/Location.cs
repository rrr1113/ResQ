using System.Text.Json.Serialization;
using Domain.Common;
using Domain.Dto;

namespace Domain.Models;

public class Location : BaseEntity
{
    public string Address { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    
    [JsonIgnore]
    public ICollection<Incident> Incidents { get; set; } = new List<Incident>();
    public ICollection<ResponseTeam> ResponseTeams { get; set; } = new List<ResponseTeam>();
    public ICollection<WeatherSnapshot> WeatherSnapshots { get; set; } = new List<WeatherSnapshot>();
}