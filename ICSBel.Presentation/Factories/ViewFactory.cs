using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace ICSBel.Presentation.Factories;

internal class ViewFactory
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<ViewFactory> _logger;

    public ViewFactory(IServiceProvider serviceProvider, ILogger<ViewFactory> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public Form CreateView<TView>() where TView : Form
    {
        if (_serviceProvider.GetService<TView>() is Form view)
        {
            _logger.LogInformation("Представление типа '{0}' создано успешно", typeof(TView).ToString());
            return view;
        }
        else
        {
            _logger.LogError("Представление типа '{0}' не найдено", typeof(TView).ToString());
            return null;
        }
    }
}