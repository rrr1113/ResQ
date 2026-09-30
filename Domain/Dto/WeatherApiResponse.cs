using System.Text.Json.Serialization;

namespace Domain.Dto;

public class WeatherApiResponse
{
    [JsonPropertyName("current")]
    public WeatherCurrent CurrentWeatherData { get; set; }
    
    [JsonPropertyName("elevation")]
    public decimal Elevation { get; set; }
   
}

public class WeatherCurrent
{
    [JsonPropertyName("temperature_2m")]
    public decimal Temperature {get; set;}
    
    [JsonPropertyName("wind_speed_10m")]
    public decimal WindSpeed {get; set;}
    
    [JsonPropertyName("precipitation")]
    public decimal Precipitation {get; set;}
    
    [JsonPropertyName("weather_code")]
    public decimal WeatherCode {get; set;}
    
}



/*
{
  "latitude": 42,
  "longitude": 21.4375,
  "generationtime_ms": 0.0641345977783203,
  "utc_offset_seconds": 0,
  "timezone": "GMT",
  "timezone_abbreviation": "GMT",
  "elevation": 247,
  "current_units": {
    "time": "iso8601",
    "interval": "seconds",
    "temperature_2m": "°C",
    "precipitation": "mm",
    "wind_speed_10m": "km/h",
    "weather_code": "wmo code"
  },
  "current": {
    "time": "2026-09-26T23:00",
    "interval": 900,
    "temperature_2m": 12.6,
    "precipitation": 0,
    "wind_speed_10m": 5.2,
    "weather_code": 2
  }
}
*/