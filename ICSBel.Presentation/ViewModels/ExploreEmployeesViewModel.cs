using System.ComponentModel;
using System.Runtime.CompilerServices;
using ICSBel.Domain.Models;
using ICSBel.Domain.Services;

namespace ICSBel.Presentation.ViewModels;

public class ExploreEmployeesViewModel : INotifyPropertyChanged
{
    private EmployeeDataService _employeeDataService;
    private BindingList<Employee> _employees;
    private BindingList<Position> _positions;
    private Stack<Employee> _selectedEmployees;
        
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
        
        
        
    public Stack<Employee> SelectedEmployees
    {
        get => _selectedEmployees;
        set
        {
            _selectedEmployees = value;
            OnPropertyChanged();
        }
    }
        
    public event PropertyChangedEventHandler PropertyChanged;

    public ExploreEmployeesViewModel(EmployeeDataService employeeDataService)
    {
        _employeeDataService = employeeDataService;
            
        _employees = new BindingList<Employee>(_employeeDataService
            .GetEmployeeRepository()
            .GetAllEmployees()
            .ToList());
            
        _positions = new BindingList<Position>(_employeeDataService
            .GetEmployeeRepository()
            .GetAllPositions()
            .ToList());
    }


    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}