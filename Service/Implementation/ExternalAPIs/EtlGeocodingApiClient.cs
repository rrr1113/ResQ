using System.Net.Http.Json;
using Domain.Dto;
using Service.Interface;

namespace Service.Implementation;

public class EtlGeocodingApiClient : IGeocodingApiClient
{
    private readonly HttpClient _httpClient;

    public EtlGeocodingApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<GeocodingApiResult?> GetLongitudeAndLatitudeForAddress(string address, string city, string country)
    {
        var raw = await ExtractAsync(address, city, country);
        return Transform(raw);
    }

    private async Task<List<GeocodingApiResponse>?> ExtractAsync(string address, string city, string country)
    {
        var query = Uri.EscapeDataString($"{address}, {city}, {country}");
        var url = $"search?q={query}&format=json&limit=1";

        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<List<GeocodingApiResponse>>();
    }

    private static GeocodingApiResult? Transform(List<GeocodingApiResponse>? raw)
    {
        var match = raw?.FirstOrDefault();

        if (match is null)
            return null;
        
        return new GeocodingApiResult
        {
            Latitude = match.Latitude,
            Longitude = match.Longitude
        };
    }
}