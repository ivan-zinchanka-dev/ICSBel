using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ICSBel.Domain.Models;
using ICSBel.Domain.Services;
using ICSBel.Presentation.Base;
using ICSBel.Presentation.Factories;
using ICSBel.Presentation.Views;
using Microsoft.Extensions.Logging;

namespace ICSBel.Presentation.ViewModels;

internal class ExploreEmployeesViewModel : INotifyPropertyChanged
{
    private readonly ILogger<ExploreEmployeesViewModel> _logger;
    private readonly EmployeeDataService _employeeDataService;
    private readonly ViewFactory _viewFactory;
    
    private BindingList<Employee> _employees;
    private BindingList<Position> _positions;
    
    public BindingList<Employee> AllEmployees
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


    public ICommand AddEmployeeCommand { get; }
    public ICommand RemoveEmployeesCommand { get; }

    public event PropertyChangedEventHandler PropertyChanged;

    public ExploreEmployeesViewModel(
        ILogger<ExploreEmployeesViewModel> logger,
        EmployeeDataService employeeDataService, 
        ViewFactory viewFactory)
    {
        _employeeDataService = employeeDataService;
        _viewFactory = viewFactory;
        _logger = logger;
        
        AddEmployeeCommand = new RelayCommand(OpenAddEmployeeDialog);
        RemoveEmployeesCommand = new RelayCommand(RemoveEmployeesAsync);
    }

    public async Task InitializeAsync()
    {
        IEnumerable<Position> positions = await _employeeDataService.PositionRepository.GetPositionsAsync();
        IEnumerable<Employee> employees = await _employeeDataService.EmployeeRepository.GetAllEmployeesAsync();
        
        Positions = new BindingList<Position>(positions.ToList());
        AllEmployees =  new BindingList<Employee>(employees.ToList());
    }

    private void OpenAddEmployeeDialog(object param)
    {
        var newEmployeeDialog = _viewFactory.CreateView<NewEmployeeView>();

        if (newEmployeeDialog != null)
        {
            DialogResult result = newEmployeeDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                //IEnumerable<Employee> employees = await _employeeDataService.EmployeeRepository.GetAllEmployeesAsync();
                
                //AllEmployees =  new BindingList<Employee>(employees.ToList());
                
                /*AllEmployees = new BindingList<Employee>(_employeeDataService
                    .GetEmployeeRepository()
                    .GetAllEmployees()
                    .ToList());*/
            }

            newEmployeeDialog.Close();
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
            _logger.LogError(ex, "An unhandled exception occurred");
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
        IEnumerable<Employee> employees = await _employeeDataService.EmployeeRepository.GetAllEmployeesAsync();
        AllEmployees =  new BindingList<Employee>(employees.ToList());
    }

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}