using EventFlow.Aggregates;
using WSiteEventFlow.Core.Aggregates.Entities;

namespace WSiteEventFlow.Core.Aggregates.Events
{
    public class EmployeeSagaStartRequestedEvent : AggregateEvent<EmployeeAggregate, EmployeeId>
    {
    }
}
