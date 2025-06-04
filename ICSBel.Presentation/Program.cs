using ICSBel.Presentation.Views;

namespace ICSBel.Presentation;

public static class Program
{
    [STAThread]
    public static void Main()
    {
        ApplicationConfiguration.Initialize();
        Application.Run(new ExploreEmployeesView());
    }
}