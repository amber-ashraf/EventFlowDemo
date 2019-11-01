using System;
using System.Collections.Generic;
using System.Text;

namespace WSiteEventFlow.Core.Aggregates.Queries
{
    public interface IScopedContext
    {
        string Id { get; }
    }
}
