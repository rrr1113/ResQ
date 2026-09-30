using System.Globalization;
using System.Net.Http.Json;
using Domain.Dto;
using Domain.Models;
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
    
    public async Task<WeatherSnapshot?> GetWeatherForecastForLongitudeAndLatitude(Guid locationId)
    {
        Location location = await _locationService.GetByIdNotNullAsync(locationId);
        
        if (location.Longitude == null || location.Latitude == null)
        {
            Console.WriteLine("NO COORDINATES - WEATHER API NOT CALLED");
            return null;
        }
        
        String latitude = location.Latitude.Value.ToString(CultureInfo.InvariantCulture);
        String longitude = location.Longitude.Value.ToString(CultureInfo.InvariantCulture);
        
        var url = $"v1/forecast?latitude={latitude}&longitude={longitude}" +
                  "&current=temperature_2m,precipitation,wind_speed_10m,weather_code";

        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var weatherData = await response.Content.ReadFromJsonAsync<WeatherApiResponse>();
        if (weatherData == null || weatherData.CurrentWeatherData == null)
        {
            return null;
        }
        return new WeatherSnapshot()
        {
            LocationId = locationId,
            Location = location,
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