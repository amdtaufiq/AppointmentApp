using AppointmentApp.Domain.Entities;
using AppointmentApp.Infra.Data.Data;
using AppointmentApp.Infra.Data.Repositories.Queries;
using Microsoft.EntityFrameworkCore;

namespace AppointmentApp.Infra.Data.Tests.Repositories
{
    public class AgencyQueryRepositoryTests
    {
        private AppointmentDBContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<AppointmentDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new AppointmentDBContext(options);
            context.Database.EnsureCreated();

            return context;
        }

        [Fact]
        public async Task IsHolidayAsync_ShouldReturnTrue_WhenDateIsHoliday()
        {
            // Arrange
            var context = CreateInMemoryContext();
            var repository = new AgencyQueryRepository(context);

            var agencyHoliday = new AgencyHoliday
            {
                AgencyId = 1,
                Description = "Week",
                HolidayDate = DateTime.Today
            };

            await context.AgencyHolidays.AddAsync(agencyHoliday);
            await context.SaveChangesAsync();

            // Act
            var result = await repository.IsHolidayAsync(1, DateTime.Today);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task GetMaxAppointmentsPerDayAsync_ShouldReturnMaxAppointments()
        {
            // Arrange
            var context = CreateInMemoryContext();
            var repository = new AgencyQueryRepository(context);

            var agencySetting = new AgencySetting
            {
                AgencyId = 2,
                MaxAppointmentsPerDay = 5
            };

            await context.AgencySettings.AddAsync(agencySetting);
            await context.SaveChangesAsync();

            // Act
            var maxAppointments = await repository.GetMaxAppointmentsPerDayAsync(2);

            // Assert
            Assert.Equal(5, maxAppointments);
        }

        [Fact]
        public async Task GetAppointmentsCountAsync_ShouldReturnCorrectCount()
        {
            // Arrange
            var context = CreateInMemoryContext();
            var repository = new AgencyQueryRepository(context);

            await context.Appointments.AddRangeAsync(
                new Appointment { AgencyId = 3, CustomerName = "1", CustomerPhoneNumber = "1", Token = "1", AppointmentDate = DateTime.Today },
                new Appointment { AgencyId = 3, CustomerName = "1", CustomerPhoneNumber = "1", Token = "1", AppointmentDate = DateTime.Today },
                new Appointment { AgencyId = 4, CustomerName = "1", CustomerPhoneNumber = "1", Token = "1", AppointmentDate = DateTime.Today }
            );

            await context.SaveChangesAsync();

            // Act
            var count = await repository.GetAppointmentsCountAsync(3, DateTime.Today);

            // Assert
            Assert.Equal(2, count);
        }
    }
}
