using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Application.CQRS.Commands;
using Application.CQRS.Queries;
using Domain.Entities;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using Moq;
using PermissionsAPI.Controllers;

namespace PermissionsAPI.Tests.PermissionsAPI
{
    public class PermissionsControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly PermissionsController _controller;

        public PermissionsControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new PermissionsController(_mediatorMock.Object);
        }

        [Fact]
        public async Task RequestPermission_ShouldReturnSuccess()
        {
            // Arrange
            var command = new RequestPermissionCommand
            {
                EmployeeForename = "Juan",
                EmployeeSurname = "Perez",
                PermissionTypeId = 2,
                PermissionDate = DateTime.UtcNow
            };

            _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
                         .ReturnsAsync(Unit.Value);

            // Act
            var result = await _controller.RequestPermission(command) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal("Permission requested successfully.", result.Value);
        }
        [Fact]
        public async Task ModifyPermission_ShouldReturnSuccess()
        {
            // Arrange
            var command = new ModifyPermissionCommand { NewPermissionTypeId = 3, UpdatedDate = DateTime.UtcNow };
            _mediatorMock.Setup(m => m.Send(It.IsAny<ModifyPermissionCommand>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(Unit.Value);

            // Act
            var result = await _controller.ModifyPermission(1, command) as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal("Permission modified successfully.", result.Value);
        }
        [Fact]
        public async Task GetPermissions_ShouldReturnPermissionsList()
        {
            // Arrange
            var permissions = new List<Permission>
                {
                     new Permission { Id = 1, EmployeeForename = "Juan", PermissionTypeId = 2, PermissionDate = DateTime.UtcNow }
                 };

            _mediatorMock.Setup(m => m.Send(It.IsAny<GetPermissionsQuery>(), It.IsAny<CancellationToken>()))
                         .ReturnsAsync(permissions);

            // Act
            var result = await _controller.GetPermissions() as OkObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.Equal(permissions, result.Value);
        }


    }
}
