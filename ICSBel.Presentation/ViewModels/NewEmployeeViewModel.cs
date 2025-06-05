using System.Windows.Input;
using ICSBel.Domain.Models;
using ICSBel.Domain.Services;
using ICSBel.Presentation.Base;

namespace ICSBel.Presentation.ViewModels;

internal class NewEmployeeViewModel : BaseViewModel
{
    private readonly EmployeeDataService _employeeDataService;

    private string _firstName = string.Empty;
    private string _lastName = string.Empty;
    private int _positionId;
    private int _birthYear;
    private decimal _salary;

    public string FirstName
    {
        get => _firstName;
        set
        {
            _firstName = value; 
            OnPropertyChanged();
        }
    }

    public string LastName
    {
        get => _lastName;
        set
        {
            _lastName = value; 
            OnPropertyChanged();
        }
    }

    public int PositionId
    {
        get => _positionId;
        set
        {
            _positionId = value; 
            OnPropertyChanged();
        }
    }

    public int BirthYear
    {
        get => _birthYear;
        set
        {
            _birthYear = value; 
            OnPropertyChanged();
        }
    }

    public decimal Salary
    {
        get => _salary;
        set
        {
            _salary = value; 
            OnPropertyChanged();
        }
    }

    public ICommand SubmitCommand => new RelayCommand(Submit);
    public ICommand CancelCommand => new RelayCommand(Cancel);
    
    public NewEmployeeViewModel(EmployeeDataService employeeDataService)
    {
        _employeeDataService = employeeDataService;
    }

    private void Submit(object param)
    {
        _employeeDataService
            .GetEmployeeRepository()
            .AddEmployee(new EmployeeInputData(FirstName, LastName, PositionId, BirthYear, Salary));
    }
    
    private void Cancel(object param)
    {
    }
}