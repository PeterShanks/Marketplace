using Marketplace.EventSourcing;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Marketplace.WebApi;

[ApiController]
public abstract class CommandApi<T> : ControllerBase
    where T : AggregateRoot
{
    private readonly ApplicationService<T> _applicationService;
    private readonly ILogger _logger;

    protected CommandApi(
        ILogger logger,
        ApplicationService<T> applicationService)
    {
        _logger = logger;
        _applicationService = applicationService;
    }

    protected async Task<IActionResult> HandleCommand<TCommand>(
        TCommand command,
        Action<TCommand>? commandModifier = null)
        where TCommand : class
    {
        try
        {
            _logger.LogDebug(
                "Handling HTTP request of type {Type}",
                typeof(T).Name
            );

            commandModifier?.Invoke(command);
            await _applicationService.Handle(command);
            return new OkResult();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error handling the command");

            return new BadRequestObjectResult(
                new
                {
                    error = e.Message,
                    stackTrace = e.StackTrace
                }
            );
        }
    }

    protected Guid GetUserId()
    {
        return Guid.Parse(User.Identity?.Name);
    }
}