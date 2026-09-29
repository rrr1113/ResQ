using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Service.Interface.Excel;

namespace Web.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ExportController : ControllerBase
{
    private readonly IExcelExportService _excelExportService;
    private const string XlsxContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

    public ExportController(IExcelExportService excelExportService)
    {
        _excelExportService = excelExportService;
    } 
    
    [HttpGet("{country}-{city}/incidents.xlsx")]
    public async Task<IActionResult> ExportIncidents([FromQuery] String city, [FromQuery] String country)
    {
        var bytes = await _excelExportService.ExportIncidentsToExcel(city, country);
        return File(bytes, XlsxContentType, $"incidents_{city}.xlsx");
    }

    [HttpGet("team-performance.xlsx")]
    public async Task<IActionResult> ExportTeamPerformance([FromQuery] Guid emergencyServiceId)
    {
        var bytes = await _excelExportService.ExportResponseTeamToExcel(emergencyServiceId);
        return File(bytes, XlsxContentType, $"team-performance_{emergencyServiceId}.xlsx");
    }
}