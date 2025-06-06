using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ICSBel.Domain.API;
using ICSBel.Domain.Models;
using ICSBel.Domain.Validation;
using ICSBel.Domain.Validation.Attributes;
using ICSBel.Presentation.Base;

namespace ICSBel.Presentation.ViewModels;

internal class NewEmployeeViewModel : BaseViewModel
{
    private readonly EmployeeDataService _employeeDataService;

    private string _firstName = string.Empty;
    private string _lastName = string.Empty;
    private BindingList<Position> _positions;
    private Position _selectedPosition;
    private int _birthYear = 2_000;
    private decimal _salary = 1_000;
    
    private readonly ValidationErrorCollection _validationErrors = new ValidationErrorCollection();
    
    private static class Messages
    {
        public const string FirstNameErrorMessage = "The first name must not be empty";
        public const string LastNameErrorMessage = "The last name must not be empty";
    }
    
    [Required(AllowEmptyStrings = false, ErrorMessage = Messages.FirstNameErrorMessage)]
    public string FirstName
    {
        get => _firstName;
        set
        {
            _firstName = value; 
            OnPropertyChanged();
            ValidateProperty(value);
        }
    }

    [Required(AllowEmptyStrings = false, ErrorMessage = Messages.LastNameErrorMessage)]
    public string LastName
    {
        get => _lastName;
        set
        {
            _lastName = value; 
            OnPropertyChanged();
            ValidateProperty(value);
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
    
    [Required]
    public Position SelectedPosition
    {
        get => _selectedPosition;
        set
        {
            _selectedPosition = value;
            OnPropertyChanged();
            ValidateProperty(value);
        }
    }

    [CorrectBirthYear]
    public int BirthYear
    {
        get => _birthYear;
        set
        {
            _birthYear = value; 
            OnPropertyChanged();
            ValidateProperty(value);
        }
    }

    [CorrectSalary]
    public decimal Salary
    {
        get => _salary;
        set
        {
            _salary = value; 
            OnPropertyChanged();
            ValidateProperty(value);
        }
    }
    
    public ICommand SubmitCommand => new RelayCommand(SubmitAsync, CanSubmit);
    public ICommand CancelCommand => new RelayCommand(Cancel);
    
    public NewEmployeeViewModel(EmployeeDataService employeeDataService)
    {
        _employeeDataService = employeeDataService;
    }

    public async Task InitializeAsync()
    {
        IEnumerable<Position> positions = await _employeeDataService.PositionRepository.GetPositionsAsync();
        Positions = new BindingList<Position>(positions.ToList());
        SelectedPosition = Positions.FirstOrDefault();
    }
    
    public override bool HasErrors => _validationErrors.Any();

    private bool CanSubmit(object param)
    {
        ValidateAllProperties();
        return !HasErrors;
    }

    public override IEnumerable GetErrors(string propertyName)
    {
        if (!string.IsNullOrEmpty(propertyName) && !string.IsNullOrWhiteSpace(propertyName))
        {
            return _validationErrors.GetPropertyErrors(propertyName);
        }
        
        return Array.Empty<string>();
    }
    
    private void ValidateProperty(object value, [CallerMemberName] string propertyName = null)
    {
        var context = new ValidationContext(this)
        {
            MemberName = propertyName
        };

        var results = new List<ValidationResult>();
        Validator.TryValidateProperty(value, context, results);

        if (results.Any())
        {
            foreach (ValidationResult result in results)
            {
                _validationErrors.TryAddPropertyError(propertyName, result.ErrorMessage);
            }
        }
        else
        {
            _validationErrors.TryClearPropertyErrors(propertyName);
        }
        
        OnErrorsChanged(propertyName);
    }
    
    public void ValidateAllProperties()
    {
        var context = new ValidationContext(this);
        var results = new List<ValidationResult>();
        Validator.TryValidateObject(this, context, results, true);

        _validationErrors.ClearAllErrors();

        foreach (ValidationResult result in results)
        {
            foreach (string memberName in result.MemberNames)
            {
                _validationErrors.TryAddPropertyError(memberName, result.ErrorMessage);
                OnErrorsChanged(memberName);
            }
        }
    }
    
    private async void SubmitAsync(object param)
    {
        Console.WriteLine("SUBMIT");
        
        bool result = await _employeeDataService
            .EmployeeRepository
            .AddEmployeeAsync(new EmployeeInputData(FirstName, LastName, SelectedPosition.Id, BirthYear, Salary));
    }
    
    private void Cancel(object param)
    {
    }
}