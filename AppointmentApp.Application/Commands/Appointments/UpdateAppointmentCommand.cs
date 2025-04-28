using MediatR;

namespace AppointmentApp.Application.Commands.Appointments
{
    public class UpdateAppointmentCommand : IRequest<Unit>
    {
        public string Token { get; }

        public UpdateAppointmentCommand(string token)
        {
            Token = token;
        }
    }
}
