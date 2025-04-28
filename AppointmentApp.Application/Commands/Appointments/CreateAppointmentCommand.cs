using AppointmentApp.Application.Requests.Appointments;
using AppointmentApp.Application.Responses.Appointments;
using MediatR;

namespace AppointmentApp.Application.Commands.Appointments
{
    public class CreateAppointmentCommand : IRequest<CreateAppointmentResponse>
    {
        public CreateAppointmentRequest CreateAppointmentRequest { get; set; }

        public CreateAppointmentCommand(CreateAppointmentRequest request)
        {
            CreateAppointmentRequest = request;
        }
    }
}
