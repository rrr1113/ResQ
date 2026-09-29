using System.Net.Http.Json;
using Domain.Configuration;
using Domain.Dto;
using Domain.Models;
using Microsoft.Extensions.Options;
using Service.Interface;

namespace Service.Implementation;

public class WeatherSnapshotApiClient : IWeatherSnapshotApiClient
{
    private readonly HttpClient _httpClient;
    private readonly ILocationService _locationService;

    public WeatherSnapshotApiClient(HttpClient httpClient,
        ILocationService locationService)
    {
        _httpClient = httpClient;
        _locationService = locationService;
    }
    
    public async Task<WeatherSnapshot> GetWeatherForecastForLongitudeAndLatitude(Guid locationId)
    {
        Location location = await _locationService.GetByIdNotNullAsync(locationId);

        if (location.Longitude == null || location.Latitude == null)
        {
            throw new Exception("Location not found");
        }
        
        double latitude = location.Latitude.Value;
        double longitude = location.Longitude.Value;
        
        var url = $"v1/forecast?latitude={latitude}&longitude={longitude}" +
                  "&current=temperature_2m,precipitation,wind_speed_10m,weather_code";

        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var weatherData = await response.Content.ReadFromJsonAsync<WeatherApiResponse>();

        return new WeatherSnapshot()
        {
            LocationId = locationId,
            Temperature = (double)weatherData.CurrentWeatherData.Temperature,
            WindSpeed = (double)weatherData.CurrentWeatherData.WindSpeed,
            Rain = (double)weatherData.CurrentWeatherData.Precipitation,
            WeatherCode = (int)weatherData.CurrentWeatherData.WeatherCode,
            FetchedAt = DateTime.UtcNow,
            IsSevere = (double)weatherData.CurrentWeatherData.WindSpeed > 50
                         || (double)weatherData.CurrentWeatherData.Precipitation > 10
                         || (int)weatherData.CurrentWeatherData.WeatherCode >= 95
        };
    }
}