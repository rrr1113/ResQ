using Domain.Configuration;
using Domain.Dto;
using Domain.Models;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Service.Interface;

namespace Service.Implementation;

public class WeatherSnapshotService : IWeatherSnapshotService
{
    private readonly IMemoryCache _memoryCache;
    private readonly IWeatherSnapshotApiClient _weatherApiClient;
    private readonly WeatherApiSettings _weatherApiSettings;

    public WeatherSnapshotService(IMemoryCache memoryCache, 
        IWeatherSnapshotApiClient weatherApiClient,
        IOptions<WeatherApiSettings> weatherApiSettings)
    {
        _memoryCache = memoryCache;
        _weatherApiClient = weatherApiClient;
        _weatherApiSettings = weatherApiSettings.Value;
    }
    
    public async Task<WeatherSnapshotDto> GetWeatherDataForLocationIdAsync(Guid locationId)
    {
        var cacheKey = $"weather-api-for-location:{locationId}";
        
        if (_memoryCache.TryGetValue(cacheKey, out WeatherSnapshotDto? cached))
        {
            return cached;
        }
        
        if (cached != null)
        {
            return cached;
        }

        var apiData = 
            await _weatherApiClient.GetWeatherForecastForLongitudeAndLatitude(locationId);

        _memoryCache.Set(cacheKey, apiData, TimeSpan.FromMinutes(_weatherApiSettings.CacheExpirationMinutes));
        
        return apiData;
    }
}
