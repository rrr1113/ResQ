namespace Domain.Configuration;

public class GeocodeApiSettings
{
    public string BaseAddress { get; set; } = string.Empty;
    public string UserEmail { get; set; } = string.Empty;
    public int TimeoutSeconds { get; set; } = 30;
}