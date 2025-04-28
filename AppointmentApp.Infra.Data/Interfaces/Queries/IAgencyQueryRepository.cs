namespace AppointmentApp.Infra.Data.Interfaces.Queries
{
    public interface IAgencyQueryRepository
    {
        Task<bool> IsHolidayAsync(long agencyId, DateTime date);
        Task<int> GetMaxAppointmentsPerDayAsync(long agencyId);
        Task<int> GetAppointmentsCountAsync(long agencyId, DateTime date);
    }
}
