using AppointmentApp.Infra.Data.Data;
using AppointmentApp.Infra.Data.Interfaces.Queries;
using Microsoft.EntityFrameworkCore;

namespace AppointmentApp.Infra.Data.Repositories.Queries
{
    public class AgencyQueryRepository : IAgencyQueryRepository
    {
        private readonly AppointmentDBContext _context;

        public AgencyQueryRepository(AppointmentDBContext context)
        {
            _context = context;
        }

        public async Task<bool> IsHolidayAsync(long agencyId, DateTime date)
        {
            return await _context.AgencyHolidays
                .AnyAsync(h => h.AgencyId == agencyId && h.HolidayDate == date.Date);
        }

        public async Task<int> GetMaxAppointmentsPerDayAsync(long agencyId)
        {
            var setting = await _context.AgencySettings
                .FirstOrDefaultAsync(s => s.AgencyId == agencyId);
            return setting?.MaxAppointmentsPerDay ?? 0;
        }

        public async Task<int> GetAppointmentsCountAsync(long agencyId, DateTime date)
        {
            return await _context.Appointments
                .CountAsync(a => a.AgencyId == agencyId && a.AppointmentDate.Date == date.Date);
        }
    }
}
