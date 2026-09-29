using Domain.Enums;
using Domain.Models;

namespace Service.Interface;

public interface IPriorityCalculatorService
{
    Task<PriorityLevel> CalculatePriorityAsync(Incident incident);
}