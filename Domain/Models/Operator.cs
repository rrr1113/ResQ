using Microsoft.AspNetCore.Identity;

namespace Domain.Models;

public class Operator : IdentityUser
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public ICollection<Incident> ReportedIncidents { get; set; } = new List<Incident>();
}