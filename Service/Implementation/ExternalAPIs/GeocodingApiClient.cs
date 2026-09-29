using System.Net.Http.Json;
using System.Web;
using Domain.Dto;
using Service.Interface;

namespace Service.Implementation;

public class GeocodingApiClient : IGeocodingApiClient
{
    private readonly HttpClient _httpClient;

    public GeocodingApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<GeocodingApiResult> GetLongitudeAndLatitudeForAddress(string address, string city, string country)
    {
        //var query = HttpUtility.UrlEncode($"{address}, {city}, {country}");
        var url = $"search?q={address}, {city}, {country}&format=json&limit=1";
            
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var locationData = await response.Content.ReadFromJsonAsync<List<GeocodingApiResponse>>();
        var match = locationData?.FirstOrDefault();

        if (match is null)
            throw new InvalidOperationException($"Could not resolve coordinates for '{address}, {city}, {country}'.");
        
        return new GeocodingApiResult()
        {
            Longitude = match.Longitude,
            Latitude = match.Latitude
        };
    }
}