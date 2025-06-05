using ICSBel.Domain.Models;
using ICSBel.Presentation.ViewModels;

namespace ICSBel.Presentation.Views;

internal partial class NewEmployeeView : Form
{
    private readonly NewEmployeeViewModel _viewModel;
    
    private TextBox _firstNameInput;
    private TextBox _lastNameInput;
    private NumericUpDown _positionIdInput;
    private NumericUpDown _birthYearInput;
    private NumericUpDown _salaryInput;
    
    private Button _submitButton;
    private Button _cancelButton;
    
    public NewEmployeeView(NewEmployeeViewModel viewModel)
    {
        _viewModel = viewModel;
        DataContext = _viewModel;
        
        InitializeComponent();
        InitializeLayout();
        SetupBindings();
    }
    
    private void InitializeLayout()
    {
        Text = "Новый сотрудник";
        StartPosition = FormStartPosition.CenterParent;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        Width = 400;
        Height = 300;
        
        var firstNameLabel = new Label { Text = "Имя:", Left = 10, Top = 20, Width = 100 };
        _firstNameInput = new TextBox { Left = 120, Top = 20, Width = 200 };

        var lastNameLabel = new Label { Text = "Фамилия:", Left = 10, Top = 60, Width = 100 };
        _lastNameInput = new TextBox { Left = 120, Top = 60, Width = 200 };

        var positionLabel = new Label { Text = "ID должности:", Left = 10, Top = 100, Width = 100 };
        _positionIdInput = new NumericUpDown { Left = 120, Top = 100, Width = 200, /*Minimum = 1, Maximum = 100 */};

        var birthYearLabel = new Label { Text = "Год рождения:", Left = 10, Top = 140, Width = 100 };
        _birthYearInput = new NumericUpDown { Left = 120, Top = 140, Width = 200, /*Minimum = 1900, Maximum = DateTime.Now.Year*/ };

        var salaryLabel = new Label { Text = "Зарплата:", Left = 10, Top = 180, Width = 100 };
        _salaryInput = new NumericUpDown { Left = 120, Top = 180, Width = 200, DecimalPlaces = 2, /*Maximum = 1_000_000*/ };

        _submitButton = new Button { Text = "Сохранить", Left = 120, Top = 220, Width = 90 };
        _cancelButton = new Button { Text = "Отмена", Left = 230, Top = 220, Width = 90 };
        
        Controls.AddRange(new Control[]
        {
            firstNameLabel, _firstNameInput,
            lastNameLabel, _lastNameInput,
            positionLabel, _positionIdInput,
            birthYearLabel, _birthYearInput,
            salaryLabel, _salaryInput,
            _submitButton, _cancelButton
        });
    }
    
    private void SetupBindings()
    {
        _firstNameInput.DataBindings.Add("Text", _viewModel, nameof(NewEmployeeViewModel.FirstName), false, DataSourceUpdateMode.OnPropertyChanged);
        _lastNameInput.DataBindings.Add("Text", _viewModel, nameof(NewEmployeeViewModel.LastName), false, DataSourceUpdateMode.OnPropertyChanged);
        _positionIdInput.DataBindings.Add("Value", _viewModel, nameof(NewEmployeeViewModel.PositionId), false, DataSourceUpdateMode.OnPropertyChanged);
        _birthYearInput.DataBindings.Add("Value", _viewModel, nameof(NewEmployeeViewModel.BirthYear), false, DataSourceUpdateMode.OnPropertyChanged);
        _salaryInput.DataBindings.Add("Value", _viewModel, nameof(NewEmployeeViewModel.Salary), false, DataSourceUpdateMode.OnPropertyChanged);
        
        _submitButton.Click += (s, e) =>
        {
            _viewModel.SubmitCommand.Execute(null);
            DialogResult = DialogResult.OK;
        };

        _cancelButton.Click += (s, e) =>
        {
            _viewModel.CancelCommand.Execute(null);
            DialogResult = DialogResult.Cancel;
        };
        
        /*_submitButton.DataBindings.Add("Command", _viewModel, nameof(NewEmployeeViewModel.SubmitCommand), true);
        _cancelButton.DataBindings.Add("Command", _viewModel, nameof(NewEmployeeViewModel.CancelCommand), true);
        */
        
    }
}