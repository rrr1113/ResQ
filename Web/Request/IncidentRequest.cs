using System.ComponentModel.DataAnnotations;

namespace Web.Request;

public record IncidentRequest(
    [Required]String Type,
    [Required]String Description, 
    [Required]String Address,
    [Required]String City,
    [Required]String Country,
    [Required]int NumberOfInjured
    );
    

public record IncidentUpdateRequest(
    [Required]String Type,
    [Required]String Description, 
    [Required]int NumberOfInjured,
    [Required]String Priority,
    [Required]String Status
);