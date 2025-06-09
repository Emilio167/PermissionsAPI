using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.CQRS.Commands;
using Application.CQRS.Handlers;
using Domain.Entities;
using Domain.Interfaces;
using Infrastructure.Messaging;
using Moq;

namespace PermissionsAPI.Tests.Aplicacion.Handlers
{
    public class ModifyPermissionHandlerTests
    {
        private readonly Mock<IPermissionRepository> _repositoryMock;
        private readonly Mock<IKafkaProducer> _kafkaProducerMock;
        private readonly ModifyPermissionHandler _handler;

        public ModifyPermissionHandlerTests()
        {
            _repositoryMock = new Mock<IPermissionRepository>();
            _kafkaProducerMock = new Mock<IKafkaProducer>();

            _handler = new ModifyPermissionHandler(_repositoryMock.Object, _kafkaProducerMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldModifyPermission_WhenValidRequest()
        {
            // Arrange
            var request = new ModifyPermissionCommand { Id = 1, NewPermissionTypeId = 2, UpdatedDate = DateTime.UtcNow };
            var permission = new Permission { Id = 1, PermissionTypeId = 1, PermissionDate = DateTime.UtcNow };

            _repositoryMock.Setup(r => r.GetByIdAsync(request.Id))
                .ReturnsAsync(permission);

            _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Permission>()))
                .Returns(Task.CompletedTask);

            _kafkaProducerMock.Setup(k => k.SendMessageAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.CompletedTask);

            // Act
            await _handler.Handle(request, CancellationToken.None);

            // Assert
            _repositoryMock.Verify(r => r.GetByIdAsync(request.Id), Times.Once);
            _repositoryMock.Verify(r => r.UpdateAsync(It.Is<Permission>(p => p.PermissionTypeId == request.NewPermissionTypeId)), Times.Once);
            _kafkaProducerMock.Verify(k => k.SendMessageAsync("permissions-topic", It.Is<string>(msg => msg.Contains("Modify requested: 1"))), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldNotModifyPermission_WhenPermissionNotFound()
        {
            // Arrange
            var request = new ModifyPermissionCommand { Id = 999, NewPermissionTypeId = 2, UpdatedDate = DateTime.UtcNow };

            _repositoryMock.Setup(r => r.GetByIdAsync(request.Id))
                .ReturnsAsync((Permission)null);

            // Act
            await _handler.Handle(request, CancellationToken.None);

            // Assert
            _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Permission>()), Times.Never);
            _kafkaProducerMock.Verify(k => k.SendMessageAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }
    }

}
