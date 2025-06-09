using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.CQRS.Handlers;
using Application.CQRS.Queries;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Messaging;
using Moq;

namespace PermissionsAPI.Tests.Aplicacion.Handlers
{
    public class GetPermissionsHandlerTests
    {
        private readonly Mock<IPermissionRepository> _repositoryMock;
        private readonly Mock<IKafkaProducer> _kafkaProducerMock;
        private readonly GetPermissionsHandler _handler;

        public GetPermissionsHandlerTests()
        {
            _repositoryMock = new Mock<IPermissionRepository>();
            _kafkaProducerMock = new Mock<IKafkaProducer>();
            _handler = new GetPermissionsHandler(_repositoryMock.Object, _kafkaProducerMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnPermissions_WhenCalled()
        {
            // Arrange
            var permissions = new List<Permission> { new Permission { Id = 1, EmployeeForename = "ReadAccess" } };
            _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(permissions);
            _kafkaProducerMock.Setup(k => k.SendMessageAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(new GetPermissionsQuery(), CancellationToken.None);

            // Assert
            Assert.NotEmpty(result);
            Assert.Equal("ReadAccess", result.First().EmployeeForename);
            _kafkaProducerMock.Verify(k => k.SendMessageAsync("permissions-topic", "Get requested"), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnEmptyList_WhenNoPermissions()
        {
            // Arrange
            _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Permission>());
            _kafkaProducerMock.Setup(k => k.SendMessageAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _handler.Handle(new GetPermissionsQuery(), CancellationToken.None);

            // Assert
            Assert.Empty(result);
            _kafkaProducerMock.Verify(k => k.SendMessageAsync("permissions-topic", "Get requested"), Times.Once);
        }
    }
}

