using System.Collections;
using System.Diagnostics;
using ICSBel.Domain.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Reporting.NETCore;

namespace ICSBel.Presentation.Reporting;

public class ReportService
{
    private const string PositionSalaryTemplate = "Report.rdlc";
    private const string PositionSalaryDataSource = "MyDataSet";
    private const string ReportingFormat = "PDF";
    private const string ReportFileName = "Отчёт_Позиция_Средняя_ЗП.pdf";
    
    private readonly ILogger<ReportService> _logger;

    public ReportService(ILogger<ReportService> logger)
    {
        _logger = logger;
    }

    public async Task CreateAndOpenSalaryReportAsync(IEnumerable<PositionSalary> positionSalaries)
    {
        string templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, PositionSalaryTemplate);

        if (!File.Exists(templatePath))
        {
            _logger.LogError(new FileNotFoundException("Expected path: ", templatePath), "Report template file not found");
            return;
        }

        byte[] pdfContent = GeneratePdfContent(templatePath, MapPositionSalaries(positionSalaries));
        
        string createdFilePath = Path.Combine(Path.GetTempPath(), ReportFileName);
        await File.WriteAllBytesAsync(createdFilePath, pdfContent);
        
        _logger.LogInformation("Generated report file: {0}", createdFilePath);

        OpenFile(createdFilePath);
    }

    private IEnumerable MapPositionSalaries(IEnumerable<PositionSalary> positionSalaries)
    {
        return positionSalaries.Select(positionSalary => new {
            Position = positionSalary.Position.Name,
            AverageSalary = positionSalary.AverageSalary
        });
    }

    private byte[] GeneratePdfContent(string templatePath, IEnumerable data)
    {
        var report = new LocalReport();
        report.ReportPath = templatePath;
        
        var dataSource = new ReportDataSource(PositionSalaryDataSource, data);
        report.DataSources.Clear();
        report.DataSources.Add(dataSource);
        
        return report.Render(ReportingFormat);
    }

    private void OpenFile(string filePath)
    {
        Process.Start(new ProcessStartInfo
        {
            FileName = filePath,
            UseShellExecute = true
        });
    }
}