using AppointmentApp.Domain.Entities;
using AppointmentApp.Infra.Data.Data;
using AppointmentApp.Infra.Data.Interfaces.Queries;
using Microsoft.EntityFrameworkCore;

namespace AppointmentApp.Infra.Data.Repositories.Queries
{
    public class AppointmentQueryRepository : IAppointmentQueryRepository
    {
        private readonly AppointmentDBContext _context;

        public AppointmentQueryRepository(AppointmentDBContext context)
        {
            _context = context;
        }

        public async Task<Appointment> GetByTokenAsync(string token)
        {
            return await _context.Appointments.Where(x => x.Token == token).FirstAsync();
        }

        public async Task<List<Appointment>> GetOutStandingAppointmentsForDateAsync(long agencyId, DateTime date)
        {
            return await _context.Appointments
                .Where(a => a.AgencyId == agencyId &&
                a.AppointmentDate.Date == date.Date && 
                a.IsActive)
                .OrderBy(a => a.Token)
                .ToListAsync();
        }

        public async Task<List<Appointment>> GetAllAppointmentsAsync(long agencyId)
        {
            return await _context.Appointments
                .Where(a => a.AgencyId == agencyId)
                .OrderBy(a => a.AppointmentDate)
                .ThenBy(a => a.Token)
                .ToListAsync();
        }

        public async Task<int> GetAppointmentsCountByDateAsync(DateTime appointmentDate)
        {
            return await _context.Appointments
                .Where(a => a.AppointmentDate.Date == appointmentDate.Date)
                .CountAsync();
        }
    }
}
