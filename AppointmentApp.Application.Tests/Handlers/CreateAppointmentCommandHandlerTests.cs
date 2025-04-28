using AppointmentApp.Application.Commands.Appointments;
using AppointmentApp.Application.Handlers.Appointments;
using AppointmentApp.Application.Requests.Appointments;
using AppointmentApp.Domain.Entities;
using AppointmentApp.Infra.Data.Interfaces.UnitOfWork;
using AutoMapper;
using Moq;

namespace AppointmentApp.Application.Tests.Handlers
{
    public class CreateAppointmentCommandHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly CreateAppointmentCommandHandler _handler;

        public CreateAppointmentCommandHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _handler = new CreateAppointmentCommandHandler(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldCreateAppointmentSuccessfully()
        {
            // Arrange
            var request = new CreateAppointmentCommand(new CreateAppointmentRequest
            {
                AgencyId = 1,
                AppointmentDate = DateTime.Today,
                CustomerName = "John Doe",
                CustomerPhoneNumber = "1"
            });

            _unitOfWorkMock.Setup(u => u.AgencyQueryRepository.IsHolidayAsync(1, DateTime.Today))
                .ReturnsAsync(false);

            _unitOfWorkMock.Setup(u => u.AgencyQueryRepository.GetMaxAppointmentsPerDayAsync(1))
                .ReturnsAsync(10);

            _unitOfWorkMock.Setup(u => u.AgencyQueryRepository.GetAppointmentsCountAsync(1, DateTime.Today))
                .ReturnsAsync(5);

            _unitOfWorkMock.Setup(u => u.AppointmentQueryRepository.GetAppointmentsCountByDateAsync(DateTime.Today))
                .ReturnsAsync(5);

            _mapperMock.Setup(m => m.Map<Appointment>(request.CreateAppointmentRequest))
                .Returns(new Appointment());

            _unitOfWorkMock.Setup(u => u.AppointmentCommandRepository.AddAsync(It.IsAny<Appointment>()))
                .Returns(Task.CompletedTask);

            _unitOfWorkMock.Setup(u => u.CommitAsync())
                .ReturnsAsync(1);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(DateTime.Today, result.AppointmentDate);
            Assert.False(string.IsNullOrEmpty(result.Token));
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenHoliday()
        {
            // Arrange
            var request = new CreateAppointmentCommand(
            new CreateAppointmentRequest
                {
                    AgencyId = 2,
                    AppointmentDate = DateTime.Today,
                    CustomerName = "John Doe",
                    CustomerPhoneNumber = "1"
            }
            );

            _unitOfWorkMock.Setup(u => u.AgencyQueryRepository.IsHolidayAsync(2, DateTime.Today))
                .ReturnsAsync(true);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(request, CancellationToken.None));
            Assert.Equal("Tidak bisa membuat janji temu pada hari libur.", exception.Message);
        }

        [Fact]
        public async Task Handle_ShouldThrowException_WhenAppointmentsFull()
        {
            // Arrange
            var request = new CreateAppointmentCommand
            (
                new CreateAppointmentRequest
                {
                    AgencyId = 3,
                    AppointmentDate = DateTime.Today,
                    CustomerName = "John Doe",
                    CustomerPhoneNumber = "1"
                }
            );

            _unitOfWorkMock.Setup(u => u.AgencyQueryRepository.IsHolidayAsync(3, DateTime.Today))
                .ReturnsAsync(false);

            _unitOfWorkMock.Setup(u => u.AgencyQueryRepository.GetMaxAppointmentsPerDayAsync(3))
                .ReturnsAsync(5);

            _unitOfWorkMock.Setup(u => u.AgencyQueryRepository.GetAppointmentsCountAsync(3, DateTime.Today))
                .ReturnsAsync(5);

            // Act & Assert
            var exception = await Assert.ThrowsAsync<Exception>(() => _handler.Handle(request, CancellationToken.None));
            Assert.Equal("Kuota janji temu untuk hari ini sudah penuh, silakan pilih hari lain.", exception.Message);
        }
    }
}
