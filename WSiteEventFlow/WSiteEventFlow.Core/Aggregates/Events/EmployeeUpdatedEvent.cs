using EventFlow.Aggregates;
using WSiteEventFlow.Core.Aggregates.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace WSiteEventFlow.Core.Aggregates.Events
{
    public class EmployeeUpdatedEvent : AggregateEvent<EmployeeAggregate, EmployeeId>
    {
        public Employee EmployeeRecord { get; }

        public EmployeeUpdatedEvent(Employee employeeRecord)
        {
            EmployeeRecord = employeeRecord;
        }
    }
}
