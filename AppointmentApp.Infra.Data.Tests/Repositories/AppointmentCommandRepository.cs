using AppointmentApp.Domain.Entities;
using AppointmentApp.Infra.Data.Data;
using AppointmentApp.Infra.Data.Repositories.Commands;
using Microsoft.EntityFrameworkCore;

namespace AppointmentApp.Infra.Data.Tests.Repositories
{
    public class AppointmentCommandRepositoryTests
    {
        private async Task<AppointmentDBContext> GetDbContextAsync()
        {
            var options = new DbContextOptionsBuilder<AppointmentDBContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new AppointmentDBContext(options);
            await context.Database.EnsureCreatedAsync();
            return context;
        }

        [Fact]
        public async Task AddAsync_Should_Add_Appointment_To_Database()
        {
            // Arrange
            var context = await GetDbContextAsync();
            var repository = new AppointmentCommandRepository(context);

            var appointment = new Appointment
            {
                Id = 1,
                AgencyId = 100,
                CustomerPhoneNumber = "123",
                AppointmentDate = DateTime.UtcNow.Date,
                CustomerName = "John Doe",
                Token = "ABC123"
            };

            // Act
            await repository.AddAsync(appointment);
            await context.SaveChangesAsync(); 

            // Assert
            var savedAppointment = await context.Appointments.FirstOrDefaultAsync(a => a.Id == appointment.Id);
            Assert.NotNull(savedAppointment);
            Assert.Equal("John Doe", savedAppointment.CustomerName);
            Assert.Equal("ABC123", savedAppointment.Token);
        }

        [Fact]
        public async Task UpdateAsync_Should_Update_Appointment_In_Database()
        {
            // Arrange
            var context = await GetDbContextAsync();
            var repository = new AppointmentCommandRepository(context);

            var appointment = new Appointment
            {
                Id = 2,
                AgencyId = 200,
                CustomerPhoneNumber = "123",
                AppointmentDate = DateTime.UtcNow.Date,
                CustomerName = "Jane Doe",
                Token = "XYZ789"
            };

            await context.Appointments.AddAsync(appointment);
            await context.SaveChangesAsync();

            // Act
            appointment.CustomerName = "Jane Smith";
            await repository.UpdateAsync(appointment);
            await context.SaveChangesAsync();

            // Assert
            var updatedAppointment = await context.Appointments.FirstOrDefaultAsync(a => a.Id == appointment.Id);
            Assert.NotNull(updatedAppointment);
            Assert.Equal("Jane Smith", updatedAppointment.CustomerName);
            Assert.Equal("XYZ789", updatedAppointment.Token);
        }
    }
}
