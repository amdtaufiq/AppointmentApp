using AppointmentApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AppointmentApp.Infra.Data.Data
{
    public static class DbSeeder
    {
        public static void Seed(IServiceProvider serviceProvider)
        {
            using var context = serviceProvider.GetRequiredService<AppointmentDBContext>();

            context.Database.Migrate();

            if (!context.Agencies.Any())
            {
                var agencies = new List<Agency>
            {
                new Agency { AgencyName = "Agency A" },
                new Agency { AgencyName = "Agency B" }
            };
                context.Agencies.AddRange(agencies);
                context.SaveChanges();
            }

            if (!context.AgencySettings.Any())
            {
                var settings = new List<AgencySetting>
            {
                new AgencySetting { AgencyId = 1, MaxAppointmentsPerDay = 5 },
                new AgencySetting { AgencyId = 2, MaxAppointmentsPerDay = 10 }
            };
                context.AgencySettings.AddRange(settings);
                context.SaveChanges();
            }

            if (!context.AgencyHolidays.Any())
            {
                var holidays = new List<AgencyHoliday>
            {
                new AgencyHoliday { AgencyId = 1, DayOfWeek = 0, Description = "Minggu Libur" },
                new AgencyHoliday { AgencyId = 1, DayOfWeek = 6, Description = "Sabtu Libur" },
                new AgencyHoliday { AgencyId = 1, HolidayDate = new DateTime(2025, 5, 1), Description = "Hari Buruh" },

                new AgencyHoliday { AgencyId = 2, DayOfWeek = 0, Description = "Minggu Libur" },
                new AgencyHoliday { AgencyId = 2, HolidayDate = new DateTime(2025, 12, 25), Description = "Natal" }
            };
                context.AgencyHolidays.AddRange(holidays);
                context.SaveChanges();
            }

            if (!context.Appointments.Any())
            {
                var appointments = new List<Appointment>
            {
                new Appointment { AgencyId = 1, CustomerName = "John Doe 1", CustomerPhoneNumber = "123", AppointmentDate = DateTime.Today, Token = "A20240428-001" },
                new Appointment { AgencyId = 1, CustomerName = "John Doe 2", CustomerPhoneNumber = "123", AppointmentDate = DateTime.Today, Token = "A20240428-002" },
                new Appointment { AgencyId = 1, CustomerName = "John Doe 3", CustomerPhoneNumber = "123", AppointmentDate = DateTime.Today, Token = "A20240428-003" },
                new Appointment { AgencyId = 1, CustomerName = "John Doe 4", CustomerPhoneNumber = "123", AppointmentDate = DateTime.Today, Token = "A20240428-004" },
                new Appointment { AgencyId = 1, CustomerName = "John Doe 5", CustomerPhoneNumber = "123", AppointmentDate = DateTime.Today, Token = "A20240428-005" },
                new Appointment { AgencyId = 2, CustomerName = "Jane Smith", CustomerPhoneNumber = "123", AppointmentDate = DateTime.Today.AddDays(1), Token = "B20240429-001" }
            };
                context.Appointments.AddRange(appointments);
                context.SaveChanges();
            }
        }
    }
}
