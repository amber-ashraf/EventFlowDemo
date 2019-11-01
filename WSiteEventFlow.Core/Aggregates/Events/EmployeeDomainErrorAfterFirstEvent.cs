using EventFlow.Aggregates;
using EventFlow.EventStores;
using WSiteEventFlow.Core.Aggregates.Entities;

namespace WSiteEventFlow.Core.Aggregates.Events
{
    [EventVersion("EmployeeDomainErrorAfterFirst", 1)]
    public class EmployeeDomainErrorAfterFirstEvent : AggregateEvent<EmployeeAggregate, EmployeeId>
    {
    }
}
