using AppointmentApp.Api.Controllers;
using AppointmentApp.Application.Commands.Appointments;
using AppointmentApp.Application.Queries;
using AppointmentApp.Application.Requests.Appointments;
using AppointmentApp.Application.Responses.Appointments;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace AppointmentApp.Api.Tests.Controllers
{
    public class AppointmentsControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly AppointmentsController _controller;

        public AppointmentsControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new AppointmentsController(_mediatorMock.Object);
        }

        [Fact]
        public async Task CreateAppointment_ShouldReturnOk_WhenAppointmentIsCreatedSuccessfully()
        {
            // Arrange
            var request = new CreateAppointmentRequest
            {
                AgencyId = 1,
                AppointmentDate = DateTime.Today,
                CustomerName = "John Doe",
                CustomerPhoneNumber = "123456789"
            };
            var command = new CreateAppointmentCommand(request);
            var response = new CreateAppointmentResponse { AppointmentDate = DateTime.Today, Token = "A20250428-001" };

            _mediatorMock.Setup(m => m.Send(command, default)).ReturnsAsync(response);

            // Act
            var result = await _controller.CreateAppointment(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var value = okResult.Value as dynamic;

            // Check message directly from value
            Assert.NotNull(value);
            Assert.Equal("Appointment created successfully.", value.message);

            // Check if Content is null as expected
            Assert.Null(value.Content);
        }


        [Fact]
        public async Task CreateAppointment_ShouldReturnBadRequest_WhenExceptionIsThrown()
        {
            // Arrange
            var request = new CreateAppointmentRequest
            {
                AgencyId = 1,
                AppointmentDate = DateTime.Today,
                CustomerName = "John Doe",
                CustomerPhoneNumber = "123456789"
            };
            var command = new CreateAppointmentCommand(request);

            _mediatorMock.Setup(m => m.Send(command, default)).ThrowsAsync(new Exception("An error occurred"));

            // Act
            var result = await _controller.CreateAppointment(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            dynamic value = badRequestResult.Value;
            Assert.Equal("An error occurred", value.message);
        }

        [Fact]
        public async Task GetAllAppointments_ShouldReturnOk_WhenAppointmentsAreFound()
        {
            // Arrange
            var agencyId = 1;
            var query = new GetAllAppointmentQuery(agencyId);
            var response = new List<GetAllAppointmentResponse>
            {
                new GetAllAppointmentResponse { AppointmentDate = DateTime.Today, Token = "A20250428-001" }
            };

            _mediatorMock.Setup(m => m.Send(query, default)).ReturnsAsync(response);

            // Act
            var result = await _controller.GetAllAppointments(agencyId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var value = Assert.IsType<List<GetAllAppointmentResponse>>(okResult.Value);
            Assert.Single(value);
            Assert.Equal(DateTime.Today, value[0].AppointmentDate);
        }

        [Fact]
        public async Task UpdateAppointment_ShouldReturnOk_WhenAppointmentIsUpdated()
        {
            // Arrange
            var token = "A20250428-001";
            var command = new UpdateAppointmentCommand(token);

            _mediatorMock.Setup(m => m.Send(command, default)).ReturnsAsync(Unit.Value);

            // Act
            var result = await _controller.UpdateAppointment(token);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            dynamic value = okResult.Value;
            Assert.Equal("Appointment updated successfully.", value.message);
        }

        [Fact]
        public async Task GetOutstandingAppointments_ShouldReturnOk_WhenAppointmentsAreFound()
        {
            // Arrange
            var request = new GetOutstandingAppointmentRequest { AgencyId = 1, Date = DateTime.Today };
            var query = new GetOutstandingAppointmentQuery(request);
            var response = new List<GetOutstandingAppointmentResponse>
            {
                new GetOutstandingAppointmentResponse { AppointmentDate = DateTime.Today, Token = "A20250428-001" }
            };

            _mediatorMock.Setup(m => m.Send(query, default)).ReturnsAsync(response);

            // Act
            var result = await _controller.GetOutstandingAppointments(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var value = Assert.IsType<List<GetOutstandingAppointmentResponse>>(okResult.Value);
            Assert.Single(value);
            Assert.Equal(DateTime.Today, value[0].AppointmentDate);
        }
    }
}