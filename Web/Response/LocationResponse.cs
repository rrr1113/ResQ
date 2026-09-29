namespace Web.Response;

public record LocationResponse
(
    Guid Id,
    String Address,
    String City,
    String Country,
    double? Latitude,
    double? Longitude
    );
    
public record LocationBasicResponse
(
    String Address,
    String City,
    String Country
);