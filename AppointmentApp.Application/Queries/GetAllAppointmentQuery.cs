using AppointmentApp.Application.Responses.Appointments;
using MediatR;

namespace AppointmentApp.Application.Queries
{
    public class GetAllAppointmentQuery : IRequest<List<GetAllAppointmentResponse>>
    {
        public long AgencyId { get; set; }
        public GetAllAppointmentQuery(long agencyId)
        {
            AgencyId = agencyId;
        }
    }
}
