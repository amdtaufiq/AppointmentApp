using AppointmentApp.Domain.Entities;
using AppointmentApp.Infra.Data.Data;
using AppointmentApp.Infra.Data.Repositories.Queries;
using Microsoft.EntityFrameworkCore;

namespace AppointmentApp.Tests.Repositories
{
    public class AppointmentQueryRepositoryTests
    {
        private AppointmentDBContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<AppointmentDBContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            var context = new AppointmentDBContext(options);
            context.Database.EnsureCreated();

            return context;
        }

        [Fact]
        public async Task GetByTokenAsync_ShouldReturnAppointment_WhenTokenExists()
        {
            // Arrange
            var context = CreateInMemoryContext();
            var repository = new AppointmentQueryRepository(context);

            var appointment = new Appointment
            {
                Token = "abc123",
                AgencyId = 1,
                CustomerName ="1",
                CustomerPhoneNumber="1",
                AppointmentDate = DateTime.Today,
                IsActive = true
            };

            await context.Appointments.AddAsync(appointment);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetByTokenAsync("abc123");

            // Assert
            Assert.NotNull(result);
            Assert.Equal("abc123", result.Token);
        }

        [Fact]
        public async Task GetOutStandingAppointmentsForDateAsync_ShouldReturnActiveAppointments()
        {
            // Arrange
            var context = CreateInMemoryContext();
            var repository = new AppointmentQueryRepository(context);

            await context.Appointments.AddRangeAsync(
                new Appointment { AgencyId = 2, CustomerName = "1", CustomerPhoneNumber = "1", AppointmentDate = DateTime.Today, IsActive = true, Token = "T1" },
                new Appointment { AgencyId = 2, CustomerName = "1", CustomerPhoneNumber = "1", AppointmentDate = DateTime.Today, IsActive = false, Token = "T2" },
                new Appointment { AgencyId = 3, CustomerName = "1", CustomerPhoneNumber = "1", AppointmentDate = DateTime.Today, IsActive = true, Token = "T3" }
            );
            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetOutStandingAppointmentsForDateAsync(2, DateTime.Today);

            // Assert
            Assert.Single(result);
            Assert.Equal("T1", result[0].Token);
        }

        [Fact]
        public async Task GetAllAppointmentsAsync_ShouldReturnAppointmentsForAgency()
        {
            // Arrange
            var context = CreateInMemoryContext();
            var repository = new AppointmentQueryRepository(context);

            await context.Appointments.AddRangeAsync(
                new Appointment { AgencyId = 4, CustomerName = "1", CustomerPhoneNumber = "1", AppointmentDate = DateTime.Today.AddDays(1), Token = "X1" },
                new Appointment { AgencyId = 4, CustomerName = "1", CustomerPhoneNumber = "1", AppointmentDate = DateTime.Today, Token = "X2" },
                new Appointment { AgencyId = 5, CustomerName = "1", CustomerPhoneNumber = "1", AppointmentDate = DateTime.Today, Token = "X3" }
            );
            await context.SaveChangesAsync();

            // Act
            var result = await repository.GetAllAppointmentsAsync(4);

            // Assert
            Assert.Equal(2, result.Count);
            Assert.Equal("X2", result[0].Token); // sorted by AppointmentDate, then Token
        }

        [Fact]
        public async Task GetAppointmentsCountByDateAsync_ShouldReturnCorrectCount()
        {
            // Arrange
            var context = CreateInMemoryContext();
            var repository = new AppointmentQueryRepository(context);

            await context.Appointments.AddRangeAsync(
                new Appointment { AgencyId = 2, CustomerName = "1", CustomerPhoneNumber = "1", AppointmentDate = DateTime.Today, IsActive = true, Token = "T1" },
                new Appointment { AgencyId = 2, CustomerName = "1", CustomerPhoneNumber = "1", AppointmentDate = DateTime.Today, IsActive = false, Token = "T2" },
                new Appointment { AgencyId = 3, CustomerName = "1", CustomerPhoneNumber = "1", AppointmentDate = DateTime.Today.AddDays(1), IsActive = true, Token = "T3" }
            );
            await context.SaveChangesAsync();

            // Act
            var count = await repository.GetAppointmentsCountByDateAsync(DateTime.Today);

            // Assert
            Assert.Equal(2, count);
        }
    }
}
