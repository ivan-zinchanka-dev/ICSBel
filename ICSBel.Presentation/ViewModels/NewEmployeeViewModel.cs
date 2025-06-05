using System.Windows.Input;
using ICSBel.Domain.Models;
using ICSBel.Domain.Services;
using ICSBel.Presentation.Base;

namespace ICSBel.Presentation.ViewModels;

internal class NewEmployeeViewModel : BaseViewModel
{
    private readonly EmployeeDataService _employeeDataService;

    public EmployeeInputData InputData { get; }

    public ICommand SubmitCommand => new RelayCommand(Submit);
    public ICommand CancelCommand => new RelayCommand(Submit);

   


    public NewEmployeeViewModel(EmployeeDataService employeeDataService)
    {
        _employeeDataService = employeeDataService;
    }

    private void Submit(object param)
    {
        _employeeDataService.GetEmployeeRepository().CreateEmployee(InputData);
    }
    
    private void Cancel(object param)
    {
    }
}