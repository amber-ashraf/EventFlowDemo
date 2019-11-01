using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EventflowApi.Read.Dto;
using EventFlow;
using EventFlow.Queries;
using WSiteEventFlow.Core.Aggregates.Entities;
using WSiteEventFlow.Core.Aggregates.Queries;
using Microsoft.AspNetCore.Mvc;

namespace EventflowApi.Read.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        protected readonly IQueryProcessor QueryProcessor;
        public EmployeeController(IQueryProcessor queryProcessor)
        {
            QueryProcessor = queryProcessor;
        }

        // GET api/Employee/5
        [HttpGet("{employeeId}")]
        public async Task<EmployeeResponseModel> Get(string employeeId)
        {
            var readModel = await QueryProcessor.ProcessAsync(
                       new EmployeeGetQuery(new EmployeeId(employeeId)), CancellationToken.None)
                       .ConfigureAwait(false);

            var response = new EmployeeResponseModel { TenantId = readModel.TenantId, Id = readModel.Id.GetGuid().ToString( ), FullName = readModel.FullName, Department = readModel.Department };
            return response;
        }

        // GET api/Employee
        [HttpGet]
        public async Task<IReadOnlyCollection<Employee>> Get()
        {
            var readModel = await QueryProcessor.ProcessAsync(
                       new EmployeeGetAllQuery(), CancellationToken.None)
                       .ConfigureAwait(false);

            return readModel.ToList<Employee>();
        }
    }
}