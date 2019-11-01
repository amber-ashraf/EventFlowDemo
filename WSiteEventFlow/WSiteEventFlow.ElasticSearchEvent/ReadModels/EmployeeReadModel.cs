using EventFlow.Aggregates;
using EventFlow.ReadStores;
using WSiteEventFlow.Core;
using WSiteEventFlow.Core.Aggregates.Entities;
using WSiteEventFlow.Core.Aggregates.Events;
using Nest;

namespace WSiteEventFlow.ElasticSearch.ReadModels
{    
    public class EmployeeReadModel : IReadModel, 
        IAmReadModelFor<EmployeeAggregate, EmployeeId, EmployeeAddedEvent>,
        IAmReadModelFor<EmployeeAggregate, EmployeeId, EmployeeUpdatedEvent>
    {
        [Keyword(
            Index = true)]
        public string TenantId { get; set; }
        public string Id { get;   set; }
        public string FullName { get;   set; }
        public string Department { get;  set; }

        public void Apply(IReadModelContext context, IDomainEvent<EmployeeAggregate, EmployeeId, EmployeeAddedEvent> domainEvent)
        {
            TenantId = domainEvent.Metadata["tenant_Id"];
            Id = domainEvent.AggregateEvent.EmployeeRecord.Id.Value;
            FullName = domainEvent.AggregateEvent.EmployeeRecord.FullName;
            Department = domainEvent.AggregateEvent.EmployeeRecord.Department;
        }

        public void Apply(IReadModelContext context, IDomainEvent<EmployeeAggregate, EmployeeId, EmployeeUpdatedEvent> domainEvent)
        {
            FullName = domainEvent.AggregateEvent.EmployeeRecord.FullName;
            Department = domainEvent.AggregateEvent.EmployeeRecord.Department;
        }

        public Employee ToEmployee()
        {
            return new Employee(EmployeeId.With(Id), FullName, Department, TenantId);
        }
    }
}
