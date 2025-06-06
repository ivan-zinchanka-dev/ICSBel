using System.ComponentModel;
using System.Windows.Input;
using ICSBel.Domain.API;
using ICSBel.Domain.Models;
using ICSBel.Presentation.Base;
using ICSBel.Presentation.ErrorHandling;
using ICSBel.Presentation.Factories;
using ICSBel.Presentation.Reporting;
using ICSBel.Presentation.Views;
using Microsoft.Extensions.Logging;

namespace ICSBel.Presentation.ViewModels;

internal class ExploreEmployeesViewModel : BaseViewModel
{
    private readonly ILogger<ExploreEmployeesViewModel> _logger;
    private readonly EmployeeDataService _employeeDataService;
    private readonly ViewFactory _viewFactory;
    private readonly ReportService _reportService;
    private readonly ErrorReporter _errorReporter;
    
    private BindingList<Employee> _employees;
    private BindingList<Position> _positions;
    private Position _selectedPosition;
    
    public BindingList<Employee> Employees
    {
        get => _employees;
        set
        {
            _employees = value;
            OnPropertyChanged();
        }
    }
        
    public BindingList<Position> Positions
    {
        get => _positions;
        set
        {
            _positions = value;
            OnPropertyChanged();
        }
    }
    
    public Position SelectedPosition
    {
        get => _selectedPosition;
        set
        {
            _selectedPosition = value;
            OnPropertyChanged();
            OnSelectedPositionChangedAsync();
        }
    }
    
    public ICommand AddEmployeeCommand { get; }
    public ICommand RemoveEmployeesCommand { get; }
    public ICommand ReportCommand { get; }
    public event Action DataLoaded;
    
    public ExploreEmployeesViewModel(
        ILogger<ExploreEmployeesViewModel> logger,
        EmployeeDataService employeeDataService, 
        ViewFactory viewFactory, 
        ReportService reportService, ErrorReporter errorReporter)
    {
        _logger = logger;
        _employeeDataService = employeeDataService;
        _viewFactory = viewFactory;
        _reportService = reportService;
        _errorReporter = errorReporter;

        AddEmployeeCommand = new RelayCommand(AddNewEmployeeAsync);
        RemoveEmployeesCommand = new RelayCommand(RemoveEmployeesAsync);
        ReportCommand = new RelayCommand(Report);

        InitializeAsync();
    }

    private async void InitializeAsync()
    {
        try
        {
            IEnumerable<Position> sourcePositions = await _employeeDataService.PositionRepository.GetPositionsAsync();
            Task<IEnumerable<Employee>> getEmployeesTask = GetEmployeesAsync();
        
            List<Position> positions = sourcePositions.ToList();
            positions.Insert(0, Position.All);
            Positions = new BindingList<Position>(positions);
        
            IEnumerable<Employee> employees = await getEmployeesTask;
            Employees =  new BindingList<Employee>(employees.ToList());
            
            DataLoaded?.Invoke();
        }
        catch (Exception ex)
        {
            string errorMessage = "При загрузке данных возникла ошибка";
            _logger.LogError(ex, errorMessage);
            _errorReporter.Report(ex, errorMessage);
        }
    }

    private async Task<IEnumerable<Employee>> GetEmployeesAsync()
    {
        if (_selectedPosition == null || _selectedPosition.Id == Position.All.Id)
        {
            return await _employeeDataService.EmployeeRepository.GetAllEmployeesAsync();
        }
        else
        {
            return await _employeeDataService.EmployeeRepository.GetFilteredEmployeesAsync(_selectedPosition.Id);
        }
    }

    private async void AddNewEmployeeAsync(object param)
    {
        try
        {
            var newEmployeeDialog = _viewFactory.CreateView<NewEmployeeView>();

            if (newEmployeeDialog != null)
            {
                DialogResult result = newEmployeeDialog.ShowDialog();

                if (result == DialogResult.OK)
                {
                    await UpdateEmployeesAsync();
                }

                newEmployeeDialog.Close();
            }
        }
        catch (Exception ex)
        {
            string errorMessage = "При добавлении сотрудника возникла ошибка";
            _logger.LogError(ex, errorMessage);
            _errorReporter.Report(ex, errorMessage);
        }
    }
    
    private async void RemoveEmployeesAsync(object rawDeletableIndices)
    {
        try
        {
            var employeeIndices = rawDeletableIndices as int[];
            await RemoveEmployeesAsync(employeeIndices);
        }
        catch (Exception ex)
        {
            string errorMessage = "При удалении сотрудников возникла ошибка";
            _logger.LogError(ex, errorMessage);
            _errorReporter.Report(ex, errorMessage);
        }
    }
    
    private async Task RemoveEmployeesAsync(int[] employeeIndices)
    {
        if (employeeIndices != null && employeeIndices.Length > 0)
        {
            var removableEmployees = new List<Employee>();
            
            foreach (int index in employeeIndices)
            {
                removableEmployees.Add(_employees[index]);
            }
            
            int[] ids = removableEmployees
                .Select(employee => employee.Id)
                .ToArray();

            await _employeeDataService.EmployeeRepository.RemoveEmployeesAsync(ids);
            await UpdateEmployeesAsync();
        }
    }

    private async Task UpdateEmployeesAsync()
    {
        IEnumerable<Employee> employees = await GetEmployeesAsync();
        Employees = new BindingList<Employee>(employees.ToList());
    }

    private async void OnSelectedPositionChangedAsync()
    {
        try
        {
            await UpdateEmployeesAsync();
        }
        catch (Exception ex)
        {
            string errorMessage = "При обновлении списка сотрудников возникла ошибка";
            _logger.LogError(ex, errorMessage);
            _errorReporter.Report(ex, errorMessage);
        }
    }
    
    private async void Report(object param)
    {
        try
        {
            IEnumerable<PositionSalary> positionSalaries = 
                await _employeeDataService.ReportService.GetPositionSalaryReportAsync();
            await _reportService.CreateAndOpenSalaryReportAsync(positionSalaries);
        }
        catch (Exception ex)
        {
            string errorMessage = "При создании отчёта возникла ошибка";
            _logger.LogError(ex, errorMessage);
            _errorReporter.Report(ex, errorMessage);
        }
    }
}