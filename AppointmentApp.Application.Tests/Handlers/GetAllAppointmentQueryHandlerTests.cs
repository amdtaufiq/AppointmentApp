using AppointmentApp.Application.Handlers.Appointments;
using AppointmentApp.Application.Queries;
using AppointmentApp.Application.Responses.Appointments;
using AppointmentApp.Domain.Entities;
using AppointmentApp.Infra.Data.Interfaces.UnitOfWork;
using AutoMapper;
using Moq;

namespace AppointmentApp.Application.Tests.Handlers
{
    public class GetAllAppointmentQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetAllAppointmentQueryHandler _handler;

        public GetAllAppointmentQueryHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetAllAppointmentQueryHandler(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnListOfAppointments()
        {
            // Arrange
            var agencyId = 1;
            var request = new GetAllAppointmentQuery(agencyId);

            var appointments = new List<Appointment>
            {
                new Appointment { Id = 1, AgencyId = agencyId, Token = "TKN001" },
                new Appointment { Id = 2, AgencyId = agencyId, Token = "TKN002" }
            };

            var appointmentResponses = new List<GetAllAppointmentResponse>
            {
                new GetAllAppointmentResponse{ CustomerName = "name 1", CustomerPhoneNumber = "081", AppointmentDate = DateTime.UtcNow.Date, Token = "TKN001" },
                new GetAllAppointmentResponse { CustomerName = "name 1", CustomerPhoneNumber = "081", AppointmentDate = DateTime.UtcNow.Date, Token = "TKN002" }
            };

            _unitOfWorkMock.Setup(u => u.AppointmentQueryRepository.GetAllAppointmentsAsync(agencyId))
                .ReturnsAsync(appointments);

            _mapperMock.Setup(m => m.Map<List<GetAllAppointmentResponse>>(appointments))
                .Returns(appointmentResponses);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("TKN001", result[0].Token);
            Assert.Equal("TKN002", result[1].Token);

            _unitOfWorkMock.Verify(u => u.AppointmentQueryRepository.GetAllAppointmentsAsync(agencyId), Times.Once);
            _mapperMock.Verify(m => m.Map<List<GetAllAppointmentResponse>>(appointments), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenNoAppointments_ShouldReturnEmptyList()
        {
            // Arrange
            var agencyId = 1;
            var request = new GetAllAppointmentQuery(agencyId);

            var appointments = new List<Appointment>();

            var appointmentResponses = new List<GetAllAppointmentResponse>();

            _unitOfWorkMock.Setup(u => u.AppointmentQueryRepository.GetAllAppointmentsAsync(agencyId))
                .ReturnsAsync(appointments);

            _mapperMock.Setup(m => m.Map<List<GetAllAppointmentResponse>>(appointments))
                .Returns(appointmentResponses);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);

            _unitOfWorkMock.Verify(u => u.AppointmentQueryRepository.GetAllAppointmentsAsync(agencyId), Times.Once);
            _mapperMock.Verify(m => m.Map<List<GetAllAppointmentResponse>>(appointments), Times.Once);
        }
    }
}
