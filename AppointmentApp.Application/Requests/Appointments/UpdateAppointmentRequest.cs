using System.Text.Json.Serialization;

namespace AppointmentApp.Application.Requests.Appointments
{
    public class UpdateAppointmentRequest
    {
        [JsonIgnore]
        public string token { get; set; }
        public bool IsAtive { get; set; }
    }
}
