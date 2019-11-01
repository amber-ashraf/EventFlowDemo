using EventFlow.Core;

namespace WSiteEventFlow.Core.Aggregates.Entities
{
    public class EmployeeId : Identity<EmployeeId>
    {
        public EmployeeId(string value) 
            : base(value)
        {

        }
    }
}
