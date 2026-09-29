using Domain.Dto;
using Domain.Models;

namespace Service.Interface;

public interface IWeatherSnapshotApiClient
{
    Task<WeatherSnapshot> GetWeatherForecastForLongitudeAndLatitude(Guid locationId);
}