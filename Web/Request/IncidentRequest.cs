using System.ComponentModel.DataAnnotations;

namespace Web.Request;

public record IncidentRequest(
    [Required]String Type,
    [Required]String Description, 
    [Required]String Address,
    [Required]String Priority,
    [Required]String City,
    [Required]String Country,
    [Required]int NumberOfInjured
    );
    

public record IncidentWithOperatorRequest(
    [Required]String Type, 
    [Required]DateTime ReportedAt,
    [Required]String Address,
    [Required]String City,
    [Required]String FullName
    );