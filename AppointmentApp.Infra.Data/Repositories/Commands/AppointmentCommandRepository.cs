using AppointmentApp.Domain.Entities;
using AppointmentApp.Infra.Data.Data;
using AppointmentApp.Infra.Data.Interfaces.Commands;

namespace AppointmentApp.Infra.Data.Repositories.Commands
{
    public class AppointmentCommandRepository : IAppointmentCommandRepository
    {
        private readonly AppointmentDBContext _context;

        public AppointmentCommandRepository(AppointmentDBContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Appointment appointment)
        {
            await _context.Appointments.AddAsync(appointment);
        }

        public async Task UpdateAsync(Appointment appointment)
        {
            _context.Appointments.Update(appointment);
            await Task.CompletedTask;
        }
    }
}
