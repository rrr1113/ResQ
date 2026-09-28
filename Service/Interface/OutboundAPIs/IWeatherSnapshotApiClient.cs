using Domain.Dto;
using Domain.Models;

namespace Service.Interface;

public interface IWeatherSnapshotApiClient
{
    Task<WeatherSnapshotDto> GetWeatherForecastForLongitudeAndLatitude(Guid locationId);
}