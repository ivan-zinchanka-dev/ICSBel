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
            _logger.LogInformation("View of type'{0}' successfully created", typeof(TView).ToString());
            return view;
        }
        else
        {
            _logger.LogError("View of type '{0}' not found", typeof(TView).ToString());
            return null;
        }
    }

}