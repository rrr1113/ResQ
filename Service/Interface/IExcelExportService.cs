namespace Service.Interface.Excel;

public interface IExcelExportService
{
    Task<byte[]> ExportIncidentsToExcel(string city, string country);
    
    Task<byte[]> ExportResponseTeamToExcel(Guid emergencyServiceId);
}