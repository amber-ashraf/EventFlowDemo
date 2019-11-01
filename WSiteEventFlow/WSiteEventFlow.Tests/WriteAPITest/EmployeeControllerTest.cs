
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using WSiteEventFlow.WriteAPI;
using Moq;
using WSiteEventFlow.WriteAPI.Controllers;
using EventFlow;
using EventFlow.Queries;
using EventFlow.ReadStores;
using WSiteEventFlow.WriteAPI.RequestModels;
using System.Threading.Tasks;
using Xunit;
using WSiteEventFlow.Core.Aggregates.Queries;
using System.Threading;
using EventFlow.Configuration;
using WSiteEventFlow.Core.Aggregates.Entities;
using WSiteEventFlow.Core.Aggregates.Commands;
using System;
using WSiteEventFlow.ElasticSearch.ReadModels;
using Shouldly;

namespace WSiteEventFlow.Tests.WriteAPITest
{
    public class EmployeeControllerTest : IDisposable
    {
        private readonly EmployeeController _employeeController;
        private readonly Mock<ICommandBus> _commandBusMock = new Mock<ICommandBus>();
        private readonly Mock<IQueryProcessor> _queryProcessorMock = new Mock<IQueryProcessor>();
        private readonly Mock<IReadModelPopulator> _readModelPopulatorMock = new Mock<IReadModelPopulator>();

        public EmployeeControllerTest()
        {
            _employeeController = new EmployeeController(_commandBusMock.Object, _queryProcessorMock.Object, _readModelPopulatorMock.Object);

        }

        public void Dispose()
        {
            _commandBusMock.Reset();
            _queryProcessorMock.Reset();
            _readModelPopulatorMock.Reset();
        }

        [Fact]
        public async Task Post_EmployeeData()
        {
            // Arrange
            var employeeModel = new EmployeeRequestModel
            {
                FullName = "Amber Ashraf",
                Department = "IT"
            };

            // Act
            var result = await _employeeController.Post(employeeModel).ConfigureAwait(false);

            // Assert
            result.ShouldSatisfyAllConditions(
                () => 
                {
                    result.ShouldNotBeNull();
                    result.Id.ShouldNotBeNull();
                },
                () => result.ShouldBeOfType<Employee>(),
                () => result.Id.Value.Contains("employee-"));
        }

        [Fact]
        public async Task Delete_EmployeeData()
        {
            // Arrange
            var employeeModel = new EmployeeRequestModel
            {
                FullName = "Amber Ashraf",
                Department = "IT"
            };
            var resultAdd = await _employeeController.Post(employeeModel).ConfigureAwait(false);

            // Act
            _employeeController.Delete(resultAdd.Id.Value);

            // Assert
            _readModelPopulatorMock.Verify(m => m.DeleteAsync(resultAdd.Id.Value, typeof(EmployeeReadModel), CancellationToken.None), Times.Once());
        }
    }
}
