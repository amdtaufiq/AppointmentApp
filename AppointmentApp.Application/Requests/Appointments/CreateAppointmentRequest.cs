namespace AppointmentApp.Application.Requests.Appointments
{
    public class CreateAppointmentRequest
    {
        public long AgencyId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhoneNumber { get; set; }
        public DateTime AppointmentDate { get; set; }
    }
}
