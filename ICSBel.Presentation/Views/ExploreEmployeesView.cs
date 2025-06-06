using ICSBel.Domain.Models;
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
        Enabled = false;
        
        InitializeComponent();
        InitializeLayout();
        SetUpBindings();
        SetUpSubscriptions();
    }

    private void InitializeLayout()
    {
        Text = "Сотрудники";
        Width = 800;
        Height = 500;
        StartPosition = FormStartPosition.CenterScreen;
        
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
        
        mainLayout.Controls.Add(_positionFilter, 0, 0);
        
        var rightPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.TopDown
        };

        _addButton = new Button { Text = "Добавить", Width = 120, Height = 28 };
        _removeButton = new Button { Text = "Удалить", Width = 120, Height = 28 };
        _reportButton = new Button { Text = "Отчёт", Width = 120, Height = 28 };
        
        rightPanel.Controls.Add(_addButton);
        rightPanel.Controls.Add(_removeButton);
        rightPanel.Controls.Add(_reportButton);

        mainLayout.Controls.Add(rightPanel, 1, 1);
        mainLayout.Padding = new Padding(left: 5, top: 0, right: 0, bottom: 5);
        
        _employeeTable = new DataGridView
        {
            Dock = DockStyle.Fill,
            ReadOnly = true,
            AllowUserToAddRows = false,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        };
        
        _employeeTable.AutoGenerateColumns = false;

        AddEmployeeTableColumn(nameof(Employee.FirstName), "Имя");
        AddEmployeeTableColumn(nameof(Employee.LastName), "Фамилия");
        AddEmployeeTableColumn(nameof(Employee.Position), "Должность");
        AddEmployeeTableColumn(nameof(Employee.BirthYear), "Год рождения");
        AddEmployeeTableColumn(nameof(Employee.Salary), "Зарплата");
        
        mainLayout.Controls.Add(_employeeTable, 0, 1);
        mainLayout.SetColumnSpan(_employeeTable, 1);
    }
    
    private void SetUpBindings()
    {
        _employeeTable.DataBindings
            .Add(nameof(DataGridView.DataSource), _viewModel, nameof(_viewModel.Employees), false, DataSourceUpdateMode.OnPropertyChanged);
        
        _positionFilter.DataBindings
            .Add(nameof(ComboBox.DataSource), _viewModel, nameof(_viewModel.Positions), true, DataSourceUpdateMode.OnPropertyChanged);
        _positionFilter.DataBindings
            .Add(nameof(ComboBox.SelectedItem), _viewModel, nameof(_viewModel.SelectedPosition), true, DataSourceUpdateMode.OnPropertyChanged);
        
        _addButton.DataBindings
            .Add(nameof(Button.Command), _viewModel, nameof(_viewModel.AddEmployeeCommand), true);
        _reportButton.DataBindings
            .Add(nameof(Button.Command), _viewModel, nameof(_viewModel.ReportCommand), true);
    }

    private void SetUpSubscriptions()
    {
        _viewModel.DataLoaded += OnViewModelDataLoaded; 
        _positionFilter.SelectedIndexChanged += OnFilterAccept;
        _removeButton.Click += OnRemoveEmployeesClick;
    }

    private void OnViewModelDataLoaded()
    {
        Enabled = true;
    }

    private void OnFilterAccept(object sender, EventArgs eventArgs)
    {
        _employeeTable.Focus();
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

    protected override void OnFormClosed(FormClosedEventArgs e)
    {
        _viewModel.DataLoaded -= OnViewModelDataLoaded;
        base.OnFormClosed(e);
    }
}