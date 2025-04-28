using AppointmentApp.Application.Commands.Appointments;
using AppointmentApp.Application.Handlers.Appointments;
using AppointmentApp.Domain.Entities;
using AppointmentApp.Infra.Data.Interfaces.UnitOfWork;
using MediatR;
using Moq;

namespace AppointmentApp.Application.Tests.Handlers
{
    public class UpdateAppointmentHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly UpdateAppointmentHandler _handler;

        public UpdateAppointmentHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _handler = new UpdateAppointmentHandler(_unitOfWorkMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldUpdateAppointmentSuccessfully()
        {
            // Arrange
            var request = new UpdateAppointmentCommand("TKN001");

            var appointmentEntity = new Appointment
            {
                Id = 1,
                Token = "TKN001",
                IsActive = true
            };

            _unitOfWorkMock.Setup(u => u.AppointmentQueryRepository.GetByTokenAsync(request.Token))
                .ReturnsAsync(appointmentEntity);

            _unitOfWorkMock.Setup(u => u.AppointmentCommandRepository.UpdateAsync(It.IsAny<Appointment>()))
                .Returns(Task.CompletedTask);

            _unitOfWorkMock.Setup(u => u.CommitAsync())
                .ReturnsAsync(1); // Simulate successful commit

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.Equal(Unit.Value, result);
            Assert.False(appointmentEntity.IsActive);

            _unitOfWorkMock.Verify(u => u.AppointmentQueryRepository.GetByTokenAsync(request.Token), Times.Once);
            _unitOfWorkMock.Verify(u => u.AppointmentCommandRepository.UpdateAsync(It.IsAny<Appointment>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenAppointmentNotFound_ShouldThrowException()
        {
            // Arrange
            var request = new UpdateAppointmentCommand("TKN001");

            _unitOfWorkMock.Setup(u => u.AppointmentQueryRepository.GetByTokenAsync(request.Token))
                .ReturnsAsync((Appointment)null); // Simulate appointment not found

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(request, CancellationToken.None));
            Assert.Equal("Appointment not found.", exception.Message);

            _unitOfWorkMock.Verify(u => u.AppointmentQueryRepository.GetByTokenAsync(request.Token), Times.Once);
            _unitOfWorkMock.Verify(u => u.AppointmentCommandRepository.UpdateAsync(It.IsAny<Appointment>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.CommitAsync(), Times.Never);
        }
    }
}
