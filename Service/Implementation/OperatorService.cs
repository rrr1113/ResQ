using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Service.Interface;

namespace Service.Implementation;

public class OperatorService : IOperatorService
{
    private readonly IHttpContextAccessor _accessor;

    public OperatorService(IHttpContextAccessor accessor)
    {
        _accessor = accessor;
    }

    public string? GetUserId()
    {
        return _accessor?.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
}