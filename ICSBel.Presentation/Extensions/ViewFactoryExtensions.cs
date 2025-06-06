using ICSBel.Presentation.Factories;
using ICSBel.Presentation.Views;

namespace ICSBel.Presentation.Extensions;

internal static class ViewFactoryExtensions
{
    public static Form CreateNewEmployeeView(this ViewFactory factory)
    {
        return factory.CreateView<NewEmployeeView>();
    }
}