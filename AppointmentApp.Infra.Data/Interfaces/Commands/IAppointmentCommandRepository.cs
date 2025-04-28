using AppointmentApp.Domain.Entities;

namespace AppointmentApp.Infra.Data.Interfaces.Commands
{
    public interface IAppointmentCommandRepository
    {
        Task AddAsync(Appointment appointment);
        Task UpdateAsync(Appointment appointment);
    }
}
