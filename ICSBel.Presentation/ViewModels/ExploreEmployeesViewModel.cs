using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ICSBel.Domain.Models;
using ICSBel.Domain.Services;
using ICSBel.Presentation.Base;

namespace ICSBel.Presentation.ViewModels;

public class ExploreEmployeesViewModel : INotifyPropertyChanged
{
    private EmployeeDataService _employeeDataService;
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


    public ICommand CreateEmployeeCommand { get; }
    public ICommand DeleteEmployeesCommand { get; }

    public event PropertyChangedEventHandler PropertyChanged;

    public ExploreEmployeesViewModel(EmployeeDataService employeeDataService)
    {
        _employeeDataService = employeeDataService;
        DeleteEmployeesCommand = new RelayCommand(DeleteEmployees);
        
        _employees = new BindingList<Employee>(_employeeDataService
            .GetEmployeeRepository()
            .GetAllEmployees()
            .ToList());
            
        _positions = new BindingList<Position>(_employeeDataService
            .GetEmployeeRepository()
            .GetAllPositions()
            .ToList());
    }
    
    
    private void DeleteEmployees(object rawDeletableIndices)
    {
        var employeeIndices = rawDeletableIndices as int[];
        DeleteEmployees(employeeIndices);
    }
    
    private void DeleteEmployees(int[] deletableIndices)
    {
        if (deletableIndices != null && deletableIndices.Length > 0)
        {
            foreach (int index in deletableIndices)
            {
                _employees.RemoveAt(index);
            }
        }
    }
    
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}