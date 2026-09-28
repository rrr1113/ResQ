using Domain.Dto;
using Domain.Models;

namespace Service.Interface;

public interface IWeatherSnapshotService
{
    Task<WeatherSnapshotDto> GetWeatherDataForLocationIdAsync(Guid eventId);
}