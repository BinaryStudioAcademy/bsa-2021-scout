using Domain.Common;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IDomainEventService
    {
        Task Publish(DomainEvent @event);
    }
}
