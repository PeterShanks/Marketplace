using System.Threading.Tasks;

namespace Marketplace.EventSourcing;

public interface IApplicationService
{
    Task Handle<TCommand>(TCommand command) where TCommand : class;
}