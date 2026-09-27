using ClosedXML.Excel;
using Service.Interface;
using Service.Interface.Excel;

namespace Service.Implementation.Excel;

public class ExcelExportService : IExcelExportService
{
    private readonly IIncidentService _incidentService;
    private readonly IResponseTeamService _responseTeamService;
    private readonly IEmergencyServiceService _emergencyServiceService;

    public ExcelExportService(IIncidentService incidentService, IResponseTeamService responseTeamService, 
        IEmergencyServiceService emergencyServiceService)
    {
        _incidentService = incidentService;
        _responseTeamService = responseTeamService;
        _emergencyServiceService = emergencyServiceService;
    }
    
    public async Task<byte[]> ExportIncidentsToExcel(string city)
    {
        var incidents = await _incidentService.GetAllByCity(city);
        
        using var workbook = new XLWorkbook();

        var ws = workbook.Worksheets.Add("Incidents_" + city);
        
        var headers = new[]
        {
            "Incident ID", "Operator", "Type",
            "ReportedAt", "Description", "NumberOfInjured", 
            "Address", "Number Assigned Teams", "Assigned Teams"
        };

        for (int i = 0; i < headers.Length; i++)
        {
            ws.Cell(1, i+1).Value = headers[i];
        }
        
        var headerRange = ws.Range(1, 1, 1, headers.Length);
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#AD95ED");
        headerRange.Style.Font.FontColor = XLColor.White;
        headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

        int row = 2;

        foreach (var incident in incidents)
        {
            ws.Cell(row, 1).Value = incident.Id.ToString();
            ws.Cell(row, 2).Value = $"{incident.Operator.FullName}";
            ws.Cell(row, 3).Value = incident.Type.ToString();
            ws.Cell(row, 4).Value = incident.ReportedAt;
            ws.Cell(row, 5).Value = incident.Description;
            ws.Cell(row, 6).Value = incident.NumberOfInjured;
            ws.Cell(row, 7).Value = incident.Location.Address;
            ws.Cell(row, 8).Value = incident.Deployments.Count.ToString();
            ws.Cell(row, 9).Value = string.Join(", ",
                incident.Deployments
                    .Select(d => d.ResponseTeam.Name)
                    .Distinct());
            
            row++;
        }
        
        ws.Column(4).Style.DateFormat.Format = "yyyy-MM-dd HH:mm";

        ws.Columns().AdjustToContents();

        ws.RangeUsed()?.SetAutoFilter();

        ws.SheetView.FreezeRows(1);
        
        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }

    public async Task<byte[]> ExportResponseTeamToExcel(Guid emergencyServiceId)
    {
        var responseTeams = await _responseTeamService.GetByEmergencyServiceId(emergencyServiceId);
        var emergencyService = await _emergencyServiceService.GetByIdAsync(emergencyServiceId);
        
        using var workbook = new XLWorkbook();

        var ws = workbook.Worksheets.Add("ResponseTeams_" + emergencyService.Name);
        
        var headers = new[]
        {
            "Team ID", "Name", "Number Of Members", "Base Location",
            "Total Deployments", "Avg. Response Time (min)", "Vehicles"
        };

        for (int i = 0; i < headers.Length; i++)
        {
            ws.Cell(1, i+1).Value = headers[i];
        }
        
        var headerRange = ws.Range(1, 1, 1, headers.Length);
        headerRange.Style.Font.Bold = true;
        headerRange.Style.Fill.BackgroundColor = XLColor.FromHtml("#AD95ED");
        headerRange.Style.Font.FontColor = XLColor.White;
        headerRange.Style.Alignment.Horizontal = XLAlignmentHorizontalValues.Center;

        int row = 2;

        foreach (var team in responseTeams)
        {
            double avgResponseTime = team.Deployments
                .Where(d => d.ArrivalTime.HasValue && d.CompletionTime.HasValue)
                .Average(d => (d.CompletionTime!.Value - d.ArrivalTime!.Value).TotalMinutes);
            
            ws.Cell(row, 1).Value = team.Id.ToString();
            ws.Cell(row, 2).Value = $"{team.Name}";
            ws.Cell(row, 3).Value = team.NumberOfMembers.ToString();
            ws.Cell(row, 4).Value = team.BaseLocation.Address + " - " + team.BaseLocation.City;
            ws.Cell(row, 5).Value = team.Deployments.Count.ToString();
            ws.Cell(row, 6).Value = avgResponseTime;
            ws.Cell(row, 6).Value = string.Join(", ",
                team.Vehicles
                    .Select(v => v.PlateNumber + "-" + v.VehicleType)
                    .Distinct());;
            
            row++;
        }
        
        ws.Columns().AdjustToContents();

        ws.RangeUsed()?.SetAutoFilter();

        ws.SheetView.FreezeRows(1);
        
        using var stream = new MemoryStream();
        workbook.SaveAs(stream);
        return stream.ToArray();
    }
}