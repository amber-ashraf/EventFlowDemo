using EventFlow.Commands;
using WSiteEventFlow.Core.Aggregates.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WSiteEventFlow.Core.Aggregates.Commands
{
    public class EmployeeUpdateCommand : Command<EmployeeAggregate, EmployeeId>
    {
        public Employee EmployeeRecord { get; }

        public EmployeeUpdateCommand(EmployeeId aggregateId, Employee employeeRecord)
            : base(aggregateId)
        {
            EmployeeRecord = employeeRecord;
        }
    }
    public class EmployeeUpdateCommandHandler : CommandHandler<EmployeeAggregate, EmployeeId, EmployeeUpdateCommand>
    {
        public override Task ExecuteAsync(EmployeeAggregate aggregate, EmployeeUpdateCommand command, CancellationToken cancellationToken)
        {
            aggregate.UpdateRecord(command.EmployeeRecord);

            return Task.FromResult(0);
        }
    }
}
