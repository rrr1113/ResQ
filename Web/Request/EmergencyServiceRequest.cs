using System.ComponentModel.DataAnnotations;

namespace Web.Request;

public record EmergencyServiceRequest(
    [Required] String Name,
    [Required] String ServiceType,
    [Required] String ContactPhone,
    [Required] String ContactEmail
    );
