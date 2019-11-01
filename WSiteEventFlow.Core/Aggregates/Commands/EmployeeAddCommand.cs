using EventFlow.Commands;
using System.Threading;
using System.Threading.Tasks;
using WSiteEventFlow.Core.Aggregates.Entities;

namespace WSiteEventFlow.Core.Aggregates.Commands
{
    public class EmployeeAddCommand : Command<EmployeeAggregate, EmployeeId>
    {
        public Employee EmployeeRecord { get; }

        public EmployeeAddCommand(EmployeeId aggregateId, Employee employeeRecord)
            : base(aggregateId)
        {
            EmployeeRecord = employeeRecord;
        }
    }
    public class EmployeeAddCommandHandler : CommandHandler<EmployeeAggregate, EmployeeId, EmployeeAddCommand>
    {
        public override Task ExecuteAsync(EmployeeAggregate aggregate, EmployeeAddCommand command, CancellationToken cancellationToken)
        {
            aggregate.AddRecord(command.EmployeeRecord);

            return Task.FromResult(0);
        }
    }
}
