using System.ComponentModel;
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

    private BindingList<Position> _positions;
    private Position _selectedPosition;
    
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
    
    public Position SelectedPosition
    {
        get => _selectedPosition;
        set
        {
            _selectedPosition = value;
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

    public BindingList<Position> Positions
    {
        get => _positions;
        set
        {
            _positions = value;
            OnPropertyChanged();
        }
    }
    
    
    public ICommand SubmitCommand => new RelayCommand(SubmitAsync);
    public ICommand CancelCommand => new RelayCommand(Cancel);
    
    public NewEmployeeViewModel(EmployeeDataService employeeDataService)
    {
        _employeeDataService = employeeDataService;
    }

    public async Task InitializeAsync()
    {
        IEnumerable<Position> positions = await _employeeDataService.PositionRepository.GetPositionsAsync();
        Positions = new BindingList<Position>(positions.ToList());
    }
    
    private async void SubmitAsync(object param)
    {
        bool result = await _employeeDataService
            .EmployeeRepository
            .AddEmployeeAsync(new EmployeeInputData(FirstName, LastName, SelectedPosition.Id, BirthYear, Salary));
    }
    
    private void Cancel(object param)
    {
    }
}