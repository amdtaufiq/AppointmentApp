using AppointmentApp.Application.Handlers.Appointments;
using AppointmentApp.Application.Queries;
using AppointmentApp.Application.Requests.Appointments;
using AppointmentApp.Application.Responses.Appointments;
using AppointmentApp.Domain.Entities;
using AppointmentApp.Infra.Data.Interfaces.UnitOfWork;
using AutoMapper;
using Moq;

namespace AppointmentApp.Application.Tests.Handlers
{
    public class GetOutstandingAppointmentQueryHandlerTests
    {
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly GetOutstandingAppointmentQueryHandler _handler;

        public GetOutstandingAppointmentQueryHandlerTests()
        {
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _handler = new GetOutstandingAppointmentQueryHandler(_unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnOutstandingAppointments()
        {
            // Arrange
            var agencyId = 1;
            var date = DateTime.Today;
            var request = new GetOutstandingAppointmentQuery
            (
                new GetOutstandingAppointmentRequest
                {
                    AgencyId = agencyId,
                    Date = date
                }
            );

            var appointments = new List<Appointment>
            {
                new Appointment { Id = 1, AgencyId = agencyId, Token = "TKN001", AppointmentDate = date },
                new Appointment { Id = 2, AgencyId = agencyId, Token = "TKN002", AppointmentDate = date }
            };

            var appointmentResponses = new List<GetOutstandingAppointmentResponse>
            {
                new GetOutstandingAppointmentResponse { Token = "TKN001", AppointmentDate = date, CustomerName = "A" },
                new GetOutstandingAppointmentResponse { Token = "TKN002", AppointmentDate = date, CustomerName = "B" }
            };

            _unitOfWorkMock.Setup(u => u.AppointmentQueryRepository.GetOutStandingAppointmentsForDateAsync(agencyId, date))
                .ReturnsAsync(appointments);

            _mapperMock.Setup(m => m.Map<List<GetOutstandingAppointmentResponse>>(appointments))
                .Returns(appointmentResponses);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, result.Count);
            Assert.Equal("TKN001", result[0].Token);
            Assert.Equal("TKN002", result[1].Token);

            _unitOfWorkMock.Verify(u => u.AppointmentQueryRepository.GetOutStandingAppointmentsForDateAsync(agencyId, date), Times.Once);
            _mapperMock.Verify(m => m.Map<List<GetOutstandingAppointmentResponse>>(appointments), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenNoOutstandingAppointments_ShouldReturnEmptyList()
        {
            // Arrange
            var agencyId = 1;
            var date = DateTime.Today;
            var request = new GetOutstandingAppointmentQuery
            (
                 new GetOutstandingAppointmentRequest
                 {
                    AgencyId = agencyId,
                    Date = date
                }
            );

            var appointments = new List<Appointment>(); // empty list

            var appointmentResponses = new List<GetOutstandingAppointmentResponse>(); // mapped to empty list

            _unitOfWorkMock.Setup(u => u.AppointmentQueryRepository.GetOutStandingAppointmentsForDateAsync(agencyId, date))
                .ReturnsAsync(appointments);

            _mapperMock.Setup(m => m.Map<List<GetOutstandingAppointmentResponse>>(appointments))
                .Returns(appointmentResponses);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Empty(result);

            _unitOfWorkMock.Verify(u => u.AppointmentQueryRepository.GetOutStandingAppointmentsForDateAsync(agencyId, date), Times.Once);
            _mapperMock.Verify(m => m.Map<List<GetOutstandingAppointmentResponse>>(appointments), Times.Once);
        }

        [Fact]
        public async Task Handle_WhenDateIsNull_ShouldUseCurrentDate()
        {
            // Arrange
            var agencyId = 1;
            var request = new GetOutstandingAppointmentQuery
            (
                new GetOutstandingAppointmentRequest
                {
                    AgencyId = agencyId,
                    Date = null // Null date, so we expect current date to be used
                }
            );

            var currentDate = DateTime.UtcNow.Date;
            var appointments = new List<Appointment>
            {
                new Appointment { Id = 1, AgencyId = agencyId, Token = "TKN001", AppointmentDate = currentDate }
            };

            var appointmentResponses = new List<GetOutstandingAppointmentResponse>
            {
                new GetOutstandingAppointmentResponse { Token = "TKN001", AppointmentDate = currentDate, CustomerName = "A" }
            };

            _unitOfWorkMock.Setup(u => u.AppointmentQueryRepository.GetOutStandingAppointmentsForDateAsync(agencyId, currentDate))
                .ReturnsAsync(appointments);

            _mapperMock.Setup(m => m.Map<List<GetOutstandingAppointmentResponse>>(appointments))
                .Returns(appointmentResponses);

            // Act
            var result = await _handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Single(result);
            Assert.Equal("TKN001", result[0].Token);

            _unitOfWorkMock.Verify(u => u.AppointmentQueryRepository.GetOutStandingAppointmentsForDateAsync(agencyId, currentDate), Times.Once);
            _mapperMock.Verify(m => m.Map<List<GetOutstandingAppointmentResponse>>(appointments), Times.Once);
        }
    }
}
