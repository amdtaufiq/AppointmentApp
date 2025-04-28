namespace AppointmentApp.Application.Responses.Appointments
{
    public class GetOutstandingAppointmentResponse
    {
        public string CustomerName { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Token { get; set; }
    }
}
