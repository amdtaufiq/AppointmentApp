namespace AppointmentApp.Application.Responses.Appointments
{
    public class GetAllAppointmentResponse
    {
        public string CustomerName { get; set; }
        public string CustomerPhoneNumber { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Token { get; set; }
    }
}
