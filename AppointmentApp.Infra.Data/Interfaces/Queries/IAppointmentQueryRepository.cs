using AppointmentApp.Domain.Entities;

namespace AppointmentApp.Infra.Data.Interfaces.Queries
{
    public interface IAppointmentQueryRepository
    {
        Task<Appointment> GetByTokenAsync(string token);
        Task<List<Appointment>> GetOutStandingAppointmentsForDateAsync(long agencyId, DateTime date);
        Task<List<Appointment>> GetAllAppointmentsAsync(long agencyId);
        Task<int> GetAppointmentsCountByDateAsync(DateTime appointmentDate);
    }
}
