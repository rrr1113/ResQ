using System;
using System.ComponentModel.DataAnnotations;

namespace Web.Request;

public record IncidentRequest(
    [Required]String Type,
    [Required]String Description, 
    [Required]String Address,
    String Priority,
    [Required]String City,
    [Required]String Country,
    [Required]int NumberOfInjured
    );
    

public record IncidentUpdateRequest(
    [Required]String Type,
    [Required]String Description, 
    [Required]int NumberOfInjured,
    [Required]DateTime ReportedAt,
    [Required]String Priority,
    [Required]String Status,
    [Required]DateTime LastStatusUpdate,
    [Required]Guid LocationId,
    [Required]String OperatorId
);