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

    public void GenerateAndOpenSalaryReport()
    {
        var data = new List<PositionSalary>
        {
            new PositionSalary { Position = "Программист", AverageSalary = 120000 },
            new PositionSalary { Position = "Аналитик", AverageSalary = 90000 },
            new PositionSalary { Position = "Менеджер", AverageSalary = 75000 }
        };
        
        string templatePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, PositionSalaryTemplate);

        if (!File.Exists(templatePath))
        {
            _logger.LogError(new FileNotFoundException("Expected path: ", templatePath), "Report template file not found");
            return;
        }

        byte[] pdfContent = GeneratePdfContent(templatePath, data);
        
        string createdFilePath = Path.Combine(Path.GetTempPath(), ReportFileName);
        File.WriteAllBytes(createdFilePath, pdfContent);
        
        _logger.LogInformation("Generated report file: {0}", createdFilePath);

        OpenFile(createdFilePath);
    }

    private byte[] GeneratePdfContent(string templatePath, List<PositionSalary> data)
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