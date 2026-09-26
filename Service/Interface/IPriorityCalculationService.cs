using Domain.Enums;
using Domain.Models;

namespace Service.Interface;

public interface IPriorityCalculationService
{
    Task<PriorityLevel> CalculatePriorityAsync(Incident incident);
}