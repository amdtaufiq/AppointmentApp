namespace AppointmentApp.Application.Requests.Appointments
{
    public class GetOutstandingAppointmentRequest
    {
        public long AgencyId { get; set; }
        public DateTime? Date { get; set; }
    }
}
