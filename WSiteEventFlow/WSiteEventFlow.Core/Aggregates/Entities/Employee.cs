using System;
using EventFlow.Entities;

namespace WSiteEventFlow.Core.Aggregates.Entities
{
    public class Employee : Entity<EmployeeId>
    {
        public string TenantId { get; set; }
        public string FullName { get; set; }
        public string Department { get; set; }

        public Employee(EmployeeId id, string fullName, string department, string tenantId)
            : base(id)
        {
            //if (string.IsNullOrEmpty(fullName)) throw new ArgumentNullException(nameof(fullName));
            //if (string.IsNullOrEmpty(department)) throw new ArgumentNullException(nameof(department));

            FullName = fullName;
            Department = department;
            TenantId = tenantId;

        }
    }
}
