using System.ComponentModel;
using System.Diagnostics;
using ICSBel.Presentation.ViewModels;

namespace ICSBel.Presentation.Views;

internal partial class ExploreEmployeesView : Form
{
    private readonly ExploreEmployeesViewModel _viewModel;

    private ComboBox _positionFilter;
    private DataGridView _employeeTable;
    private Button _addButton;
    private Button _removeButton;
    private Button _reportButton;
    
    public ExploreEmployeesView(ExploreEmployeesViewModel viewModel)
    {
        _viewModel = viewModel;
        DataContext = _viewModel;
        
        InitializeComponent();
        InitializeLayout();
        SetupBindings();
    }

    protected override async void OnLoad(EventArgs eventArgs)
    {
        base.OnLoad(eventArgs);
        await _viewModel.InitializeAsync();
    }

    private void InitializeLayout()
    {
        Text = "Сотрудники";
        Width = 800;
        Height = 500;

        var mainLayout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 2,
            RowCount = 2
        };
        mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 80));
        mainLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20));
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
        mainLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        Controls.Add(mainLayout);
        
        _positionFilter = new ComboBox
        {
            Dock = DockStyle.Fill,
            DropDownStyle = ComboBoxStyle.DropDownList,
            DisplayMember = "Name", 
            ValueMember = "Id",
        };
        
        _positionFilter.SelectedIndexChanged += OnFilterAccept;

        mainLayout.Controls.Add(_positionFilter, 0, 0);
        
        var rightPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.TopDown
        };

        _addButton= new Button { Text = "Добавить", Width = 100 };
        _removeButton = new Button { Text = "Удалить", Width = 100 };
        _reportButton = new Button { Text = "Отчёт", Width = 100 };
        
        rightPanel.Controls.Add(_addButton);
        rightPanel.Controls.Add(_removeButton);
        rightPanel.Controls.Add(_reportButton);

        mainLayout.Controls.Add(rightPanel, 1, 1);
        
        _employeeTable = new DataGridView
        {
            Dock = DockStyle.Fill,
            ReadOnly = true,
            AllowUserToAddRows = false,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        };
        
        _employeeTable.AutoGenerateColumns = false;

        AddEmployeeTableColumn("FirstName", "Имя");
        AddEmployeeTableColumn("LastName", "Фамилия");
        AddEmployeeTableColumn("Position", "Должность");
        AddEmployeeTableColumn("BirthYear", "Год рождения");
        AddEmployeeTableColumn("Salary", "Зарплата");
        
        mainLayout.Controls.Add(_employeeTable, 0, 1);
        mainLayout.SetColumnSpan(_employeeTable, 1);

        _viewModel.PropertyChanged += OnViewModelPropertyChanged;
    }

    private void OnFilterAccept(object sender, EventArgs eventArgs)
    {
        _employeeTable.Focus();
    }

    private void SetupBindings()
    {
        _employeeTable.DataBindings.Add(nameof(DataGridView.DataSource), _viewModel, nameof(_viewModel.Employees), false, DataSourceUpdateMode.OnPropertyChanged);
        
        _positionFilter.DataBindings.Add(nameof(ComboBox.DataSource), _viewModel, nameof(_viewModel.Positions), true, DataSourceUpdateMode.OnPropertyChanged);
        _positionFilter.DataBindings.Add(nameof(ComboBox.SelectedItem), _viewModel, nameof(_viewModel.SelectedPosition), true, DataSourceUpdateMode.OnPropertyChanged);
        
        _addButton.DataBindings.Add(nameof(Button.Command), _viewModel, nameof(_viewModel.AddEmployeeCommand), true);
        _removeButton.Click += OnRemoveEmployeesClick;
        _reportButton.DataBindings.Add(nameof(Button.Command), _viewModel, nameof(_viewModel.ReportCommand), true);
    }

    private void OnRemoveEmployeesClick(object sender, EventArgs eventArgs)
    {
        var deletableIndices = new int[_employeeTable.SelectedRows.Count];

        for (int i = 0; i < deletableIndices.Length; i++)
        {
            deletableIndices[i] = _employeeTable.SelectedRows[i].Index;
        }
        
        _viewModel.RemoveEmployeesCommand.Execute(deletableIndices);
    }

    private void AddEmployeeTableColumn(string dataPropertyName, string headerText)
    {
        _employeeTable.Columns.Add(new DataGridViewTextBoxColumn
        {
            DataPropertyName = dataPropertyName,
            HeaderText = headerText
        });
    }

    private void OnViewModelPropertyChanged(object sender, PropertyChangedEventArgs eventArgs)
    {
        /*if (eventArgs.PropertyName == nameof(_viewModel.SelectedPosition))
        {
            await _viewModel.UpdateEmployeesAsync();
        }*/
    }
}