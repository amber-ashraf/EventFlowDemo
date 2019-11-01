using System.Collections.Generic;
using EventFlow.Aggregates;
using EventFlow.ReadStores;
using WSiteEventFlow.Core.Aggregates.Events;

namespace WSiteEventFlow.Core.Aggregates.Locator
{
    public class EmployeeLocator : IReadModelLocator
    {
        public IEnumerable<string> GetReadModelIds(IDomainEvent domainEvent)
        {
            IAggregateEvent aggregateEvent = domainEvent.GetAggregateEvent();

            switch (aggregateEvent)
            {
                case EmployeeAddedEvent employeeRecordAddedEvent:
                    yield return employeeRecordAddedEvent.EmployeeRecord.Id.Value;
                    break;

                case EmployeeUpdatedEvent employeeRecordUpdatedEvent:
                    yield return employeeRecordUpdatedEvent.EmployeeRecord.Id.Value;
                    break;
            }
        }
    }
}
