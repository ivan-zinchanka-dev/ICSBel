﻿using System.Collections;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ICSBel.Domain.API;
using ICSBel.Domain.Models;
using ICSBel.Domain.Validation;
using ICSBel.Domain.Validation.Attributes;
using ICSBel.Presentation.Base;
using Microsoft.Extensions.Logging;

namespace ICSBel.Presentation.ViewModels;

internal class NewEmployeeViewModel : BaseViewModel
{
    private readonly ILogger<NewEmployeeViewModel> _logger;
    private readonly EmployeeDataService _employeeDataService;

    private string _firstName = string.Empty;
    private string _lastName = string.Empty;
    private BindingList<Position> _positions;
    private Position _selectedPosition;
    private int _birthYear = 2_000;
    private decimal _salary = 1_000;
    
    private readonly ValidationErrorCollection _validationErrors = new ValidationErrorCollection();
        
    [Required(AllowEmptyStrings = false, ErrorMessage = "Требуется ввести имя")]
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

    [Required(AllowEmptyStrings = false, ErrorMessage = "Требуется ввести фамилию")]
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
    public event Action<bool> OnCommandComplete;
    
    public NewEmployeeViewModel(ILogger<NewEmployeeViewModel> logger, EmployeeDataService employeeDataService)
    {
        _logger = logger;
        _employeeDataService = employeeDataService;

        InitializeAsync();
    }

    public async void InitializeAsync()
    {
        try
        {
            IEnumerable<Position> positions = await _employeeDataService.PositionRepository.GetPositionsAsync();
            Positions = new BindingList<Position>(positions.ToList());
            SelectedPosition = Positions.FirstOrDefault();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "При загрузке данных произошла ошибка");
        }
    }
    
    public override bool HasErrors => _validationErrors.Any();
    
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
    
    private void ValidateAllProperties(bool fireErrorsChanged = true)
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

                if (fireErrorsChanged)
                {
                    OnErrorsChanged(memberName);
                }
            }
        }
    }
    
    private bool CanSubmit(object param)
    {
        ValidateAllProperties(false);
        return !HasErrors;
    }
    
    private async void SubmitAsync(object param)
    {
        try
        {
            bool result = await _employeeDataService
                .EmployeeRepository
                .AddEmployeeAsync(new EmployeeInputData(FirstName, LastName, SelectedPosition.Id, BirthYear, Salary));

            OnCommandComplete?.Invoke(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "При добавлении сотрудника возникла ошибка");
            OnCommandComplete?.Invoke(false);
        }
    }
    
    private void Cancel(object param)
    {
        OnCommandComplete?.Invoke(true);
    }
}