namespace Web.Request;

public record LocationRequest
(
    String Address,
    String City,
    String Country,
    double Latitude,
    double Longitude
);

public record LocationBasicRequest
(
    String Address,
    String City,
    String Country
    );