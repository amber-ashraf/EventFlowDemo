using EventFlow.Queries;
using System;
using System.Collections.Generic;
using System.Text;
using WSiteEventFlow.Core.Aggregates.Entities;

namespace WSiteEventFlow.Core.Aggregates.Queries
{
    public class EmployeeGetAllQuery : IQuery<IReadOnlyCollection<Employee>>
    {
    }
}
