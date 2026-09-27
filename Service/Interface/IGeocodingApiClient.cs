using Domain.Dto;
using Domain.Models;

namespace Service.Interface;

public interface IGeocodingApiClient
{
    Task<GeocodingApiResult?> GetLongitudeAndLatitudeForAddress(string address, string city, string country);
}