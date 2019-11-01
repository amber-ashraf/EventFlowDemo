using EventFlow;
using EventFlow.Queries;
using EventflowApi.Read.Controllers;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using WSiteEventFlow.Core.Aggregates.Entities;
using WSiteEventFlow.Core.Aggregates.Queries;
using Xunit;
using Shouldly;

namespace WSiteEventFlow.Tests.ReadApiTest
{
    public class EmployeeControllerTest : IDisposable
    {
        private readonly EmployeeController _employeeController;
        private readonly Mock<IQueryProcessor> _queryProcessorMock = new Mock<IQueryProcessor>();

        public EmployeeControllerTest()
        {
            _employeeController = new EmployeeController(_queryProcessorMock.Object);

        }

        public void Dispose() => _queryProcessorMock.Reset();

        [Fact]
        public async Task Get_EmployeeData_ById()
        {
            // Arrange
            var GUID = Guid.NewGuid().ToString();
            var employeeId = $"employee-{GUID}";
            var employeeModel = new Employee(new EmployeeId(employeeId), "Amber Ashraf", "IT", "DummyTenant");
            _queryProcessorMock.Setup(
                x => x.ProcessAsync(It.IsAny<EmployeeGetQuery>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult<Employee>(employeeModel));

            // Act
            var result = await _employeeController.Get(employeeId).ConfigureAwait(false);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Id.Equals(GUID.ToString()));
        }

        [Fact]
        public async Task Get_AllEmployeeData()
        {
            // Arrange
            var GUID = Guid.NewGuid().ToString();
            var employeeId = $"employee-{GUID}";
            var employeeModel = new Employee(new EmployeeId(employeeId), "Amber Ashraf", "IT", "DummyTenant");
            _queryProcessorMock.Setup(
                x => x.ProcessAsync(It.IsAny<EmployeeGetAllQuery>(), It.IsAny<CancellationToken>()))
                .Returns(() => Task.FromResult<IReadOnlyCollection<Employee>>(SeedEmployeeList()));

            // Act
            var result = await _employeeController.Get().ConfigureAwait(false);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => result.ShouldNotBeNull(),
                () => result.Count.Equals(5),
                () => result.ShouldBeOfType<List<Employee>>());
        }

        private IReadOnlyCollection<Employee> SeedEmployeeList()
        {
            var employeeList = new List<Employee>();
            for(int i=0; i<5; i++)
            {
                var empData = new Employee(
                    new EmployeeId($"employee-{Guid.NewGuid().ToString()}"),
                    $"Amber-{i}",
                    $"Department-{i}",
                    $"Tenant-{i}");
                employeeList.Add(empData);
            }
            return employeeList;
        }
    }
}
