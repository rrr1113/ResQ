namespace Service.Interface.Excel;

public interface IExcelExportService
{
    Task<byte[]> ExportIncidentsToExcel(string city);
    
    Task<byte[]> ExportResponseTeamToExcel(Guid emergencyServiceId);
}