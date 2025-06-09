using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.CQRS.Commands;
using Application.CQRS.Handlers;
using Application.Services;
using Moq;

namespace PermissionsAPI.Tests.Aplicacion.Handlers
{
    public class RequestPermissionHandlerTests
    {
        private readonly Mock<IPermissionService> _serviceMock;
        private readonly RequestPermissionHandler _handler;

        public RequestPermissionHandlerTests()
        {
            _serviceMock = new Mock<IPermissionService>();
            _handler = new RequestPermissionHandler(_serviceMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldCallRequestPermissionAsync_WhenValidRequest()
        {
            // Arrange
            var request = new RequestPermissionCommand { EmployeeForename = "Emilio",EmployeeSurname="Navarro",PermissionDate=DateTime.Now, PermissionTypeId = 2 };

            _serviceMock.Setup(s => s.RequestPermissionAsync(request))
                .Returns(Task.CompletedTask);

            // Act
            await _handler.Handle(request, CancellationToken.None);

            // Assert
            _serviceMock.Verify(s => s.RequestPermissionAsync(request), Times.Once);
        }
    }
}
