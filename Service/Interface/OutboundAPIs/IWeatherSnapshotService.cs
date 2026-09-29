using Domain.Dto;

namespace Service.Interface;

public interface IWeatherSnapshotService
{
    Task<WeatherSnapshot> GetWeatherDataForLocationIdAsync(Guid eventId);
}