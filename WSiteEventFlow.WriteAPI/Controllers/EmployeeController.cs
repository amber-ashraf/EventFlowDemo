using System;
using System.Threading;
using System.Threading.Tasks;
using EventFlow;
using EventFlow.Aggregates;
using EventFlow.Queries;
using EventFlow.ReadStores;
using Microsoft.AspNetCore.Mvc;
using WSiteEventFlow.Core.Aggregates.Commands;
using WSiteEventFlow.Core.Aggregates.Entities;
using WSiteEventFlow.WriteAPI.RequestModels;
using WSiteEventFlow.ElasticSearch.ReadModels;

namespace WSiteEventFlow.WriteAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        protected readonly ICommandBus CommandBus;
        protected readonly IReadModelPopulator ReadModelPopulator;
        protected readonly IQueryProcessor QueryProcessor;
        public EmployeeController(ICommandBus commandBus,
              IQueryProcessor queryProcessor,
              IReadModelPopulator readModelPopulator)
        {
            CommandBus = commandBus;
            QueryProcessor = queryProcessor;
            ReadModelPopulator = readModelPopulator;
        }

        [HttpPost]
        public async Task<Employee> Post(EmployeeRequestModel request)
        {
            var id = Guid.NewGuid().ToString();
            var employeeId = new EmployeeId($"employee-{id}");
            var employeeData  = new Employee(employeeId, request.FullName, request.Department, "");
            var employeeCommand = new EmployeeAddCommand(employeeId, employeeData);
            await CommandBus.PublishAsync(employeeCommand, CancellationToken.None).ConfigureAwait(false);
            return employeeData;
           
        }
        // DELETE api/Employee/5
        [HttpDelete("{id}")]
        public async void Delete(string id)
        {
           await ReadModelPopulator.DeleteAsync(
                id,
                typeof(EmployeeReadModel),
                CancellationToken.None)
                .ConfigureAwait(false);
        }

        // PUT api/Employee/5
        [HttpPut("{id}")]
        public async Task<Employee> Put(string id, EmployeeRequestModel request)
        {
            var employeeId = new EmployeeId(id);
            var employeeData = new Employee(employeeId, request.FullName, request.Department, "");
            var employeeCommand = new EmployeeUpdateCommand(employeeId, employeeData);
            await CommandBus.PublishAsync(employeeCommand, CancellationToken.None).ConfigureAwait(false);
            return employeeData;
        }
    }
}