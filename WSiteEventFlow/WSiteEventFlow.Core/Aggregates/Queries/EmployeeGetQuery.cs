using EventFlow.Queries;
using WSiteEventFlow.Core.Aggregates.Entities;

namespace WSiteEventFlow.Core.Aggregates.Queries
{
    public class EmployeeGetQuery : IQuery<Employee>
    {
        public EmployeeId EmployeeId { get; }

        public EmployeeGetQuery(EmployeeId employeeId)
        {
            EmployeeId = employeeId;
        }
    }
}
