using EventFlow.Aggregates;
using WSiteEventFlow.Core.Aggregates.Entities;

namespace WSiteEventFlow.Core.Aggregates.Events
{
    public class EmployeeAddedEvent : AggregateEvent<EmployeeAggregate, EmployeeId>
    {
        public Employee EmployeeRecord { get; }

        public EmployeeAddedEvent(Employee employeeRecord)
        {
            EmployeeRecord = employeeRecord;
        }
    }
}
