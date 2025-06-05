using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using ICSBel.Domain.Models;
using ICSBel.Domain.Services;
using ICSBel.Presentation.Base;
using ICSBel.Presentation.Factories;
using ICSBel.Presentation.Views;

namespace ICSBel.Presentation.ViewModels;

internal class ExploreEmployeesViewModel : INotifyPropertyChanged
{
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

    public ExploreEmployeesViewModel(EmployeeDataService employeeDataService, ViewFactory viewFactory)
    {
        _employeeDataService = employeeDataService;
        _viewFactory = viewFactory;

        AddEmployeeCommand = new RelayCommand(OpenAddEmployeeDialog);
        RemoveEmployeesCommand = new RelayCommand(RemoveEmployees);
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


    private void RemoveEmployees(object rawDeletableIndices)
    {
        var employeeIndices = rawDeletableIndices as int[];
        RemoveEmployees(employeeIndices);
    }
    
    private void RemoveEmployees(int[] deletableIndices)
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