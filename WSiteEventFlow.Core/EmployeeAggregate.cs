using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventFlow.Aggregates;
using EventFlow.Exceptions;
using EventFlow.Snapshots;
using EventFlow.Snapshots.Strategies;
using WSiteEventFlow.Core.Aggregates.Entities;
using WSiteEventFlow.Core.Aggregates.Events;
using WSiteEventFlow.Core.Aggregates.Queries;
using WSiteEventFlow.Core.Aggregates.Snapshots;

namespace WSiteEventFlow.Core
{
    [AggregateName("Employee")]
    public class EmployeeAggregate : SnapshotAggregateRoot<EmployeeAggregate, EmployeeId, EmployeeSnapshot>, IEmit<EmployeeDomainErrorAfterFirstEvent>
    {
        private readonly IScopedContext _scopedContext;
        private readonly List<Employee> _records = new List<Employee>();

        public const int SnapshotEveryVersion = 10;

        public bool DomainErrorAfterFirstReceived { get; private set; }
        public IReadOnlyCollection<Employee> Records => _records;
        public IReadOnlyCollection<EmployeeSnapshotVersion> SnapshotVersions { get; private set; } = new EmployeeSnapshotVersion[] { };

        public EmployeeAggregate(EmployeeId id, IScopedContext scopedContext)
            : base(id, SnapshotEveryFewVersionsStrategy.With(SnapshotEveryVersion))
        {
            _scopedContext = scopedContext;

            Register<EmployeeAddedEvent>(e => _records.Add(e.EmployeeRecord));
            Register<EmployeeUpdatedEvent>(e => {
                var obj = _records.FirstOrDefault(x => x.Id == e.EmployeeRecord.Id);
                if (obj != null)
                {
                    obj.FullName = e.EmployeeRecord.FullName;
                    obj.Department = e.EmployeeRecord.Department;
                    
                }
            });
            //Register<EmployeeSagaStartRequestedEvent>(e => {/* do nothing */});
            //Register<EmployeeSagaCompleteRequestedEvent>(e => {/* do nothing */});
            //Register<EmployeeDeletedEvent>(e => _records.RemoveAll(x=> x.Id == e.EmployeeId));
        }

        public void DomainErrorAfterFirst()
        {
            if (DomainErrorAfterFirstReceived)
            {
                throw DomainError.With("DomainErrorAfterFirst already received!");
            }

            Emit(new EmployeeDomainErrorAfterFirstEvent());
        }

        public void AddRecord(Employee record)
        {
            if (_records.Any(m => m.Id == record.Id))
            {
                throw DomainError.With($"Employee '{Id}' already has a record with ID '{record.Id}'");
            }

            Emit(new EmployeeAddedEvent(record));
        }

        public void RequestSagaStart()
        {
            Emit(new EmployeeSagaStartRequestedEvent());
        }

        public void RequestSagaComplete()
        {
            Emit(new EmployeeSagaCompleteRequestedEvent());
        }

        public void Apply(EmployeeDomainErrorAfterFirstEvent e)
        {
            DomainErrorAfterFirstReceived = true;
        }

        protected override Task<EmployeeSnapshot> CreateSnapshotAsync(CancellationToken cancellationToken)
        {
            return Task.FromResult(new EmployeeSnapshot(
                Records,
                Enumerable.Empty<EmployeeSnapshotVersion>()));
        }

        protected override Task LoadSnapshotAsync(EmployeeSnapshot snapshot, ISnapshotMetadata metadata, CancellationToken cancellationToken)
        {
            _records.AddRange(snapshot.EmployeesAdded);
            SnapshotVersions = snapshot.PreviousVersions;

            return Task.FromResult(0);
        }

        public void UpdateRecord(Employee employee)
        {
            if (!_records.Any(m => m.Id == employee.Id))
            {
                throw DomainError.With($"Employee '{Id}' already has no record with ID '{employee.Id}'");
            }

            Emit(new EmployeeUpdatedEvent(employee));
        }
    }
}
