using System.ComponentModel;
using System.Diagnostics;
using ICSBel.Presentation.ViewModels;

namespace ICSBel.Presentation.Views;

internal partial class ExploreEmployeesView : Form
{
    private readonly ExploreEmployeesViewModel _viewModel;

    private DataGridView _employeeTable;
    
    public ExploreEmployeesView(ExploreEmployeesViewModel viewModel)
    {
        _viewModel = viewModel;
        DataContext = _viewModel;
        
        InitializeComponent();
        InitializeLayout();
    }
    
    private void InitializeLayout()
    {
        this.Text = "Сотрудники";
        this.Width = 800;
        this.Height = 500;

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
        this.Controls.Add(mainLayout);
        
        var positionFilter = new ComboBox
        {
            Dock = DockStyle.Fill,
            DropDownStyle = ComboBoxStyle.DropDownList
        };
        positionFilter.Items.AddRange(_viewModel.Positions.ToArray());
        positionFilter.SelectedIndex = 0;
        positionFilter.SelectedIndexChanged += (s, e) =>
        {
            // TODO: Обновить таблицу по фильтру
        };

        mainLayout.Controls.Add(positionFilter, 0, 0);
        
        var rightPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            FlowDirection = FlowDirection.TopDown
        };

        var addButton = new Button { Text = "Добавить", Width = 100 };
        var deleteButton = new Button { Text = "Удалить", Width = 100 };
        var reportButton = new Button { Text = "Отчёт", Width = 100 };
        
        
        //deleteButton.DataBindings.Add(new Binding("Command", DataContext, "AddCommand", true));
        addButton.DataBindings.Add(new Binding("Command", _viewModel, nameof(_viewModel.CreateEmployeeCommand), true));
        deleteButton.Click += OnRemoveEmployeesClick;

        rightPanel.Controls.Add(addButton);
        rightPanel.Controls.Add(deleteButton);
        rightPanel.Controls.Add(reportButton);

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

        //_employeeTable.DataSource = _viewModel.AllEmployees;
        _employeeTable.DataBindings.Add(new Binding(
            nameof(_employeeTable.DataSource), 
            _viewModel, 
            nameof(_viewModel.AllEmployees), 
            false, DataSourceUpdateMode.OnPropertyChanged));
        
        
        mainLayout.Controls.Add(_employeeTable, 0, 1);
        mainLayout.SetColumnSpan(_employeeTable, 1);

        _viewModel.PropertyChanged += OnViewModelPropertyChanged;
    }

    private void OnRemoveEmployeesClick(object sender, EventArgs eventArgs)
    {
        var deletableIndices = new int[_employeeTable.SelectedRows.Count];

        for (int i = 0; i < deletableIndices.Length; i++)
        {
            deletableIndices[i] = _employeeTable.SelectedRows[i].Index;
        }
        
        _viewModel.DeleteEmployeesCommand.Execute(deletableIndices);
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
        /*if (eventArgs.PropertyName == nameof(_viewModel.AllEmployees))
        {
            _employeeTable.DataSource = _viewModel.AllEmployees;
        }*/
    }
}