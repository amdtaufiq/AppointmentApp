using AppointmentApp.Application.Requests.Appointments;
using AppointmentApp.Application.Responses.Appointments;
using MediatR;

namespace AppointmentApp.Application.Queries
{
    public class GetOutstandingAppointmentQuery : IRequest<List<GetOutstandingAppointmentResponse>>
    {
        public GetOutstandingAppointmentRequest Outstanding { get; set; }
        
        public GetOutstandingAppointmentQuery(GetOutstandingAppointmentRequest request)
        {
            Outstanding = request;
        }
    }
}
