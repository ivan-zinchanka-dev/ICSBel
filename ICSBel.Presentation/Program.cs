using ICSBel.Domain.Services;
using ICSBel.Presentation.ViewModels;
using ICSBel.Presentation.Views;

namespace ICSBel.Presentation;

public static class Program
{
    [STAThread]
    public static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new ExploreEmployeesView(CreateEmployeesViewModel()));
    }

    private static ExploreEmployeesViewModel CreateEmployeesViewModel()
    {
        return new ExploreEmployeesViewModel(new EmployeeDataService());
    }
}