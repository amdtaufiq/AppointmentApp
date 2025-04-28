using AppointmentApp.Application.Requests.Appointments;
using AppointmentApp.Application.Responses.Appointments;
using AppointmentApp.Domain.Entities;
using AutoMapper;

namespace AppointmentApp.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<CreateAppointmentRequest, Appointment>();

            CreateMap<UpdateAppointmentRequest, Appointment>();

            CreateMap<Appointment, GetOutstandingAppointmentResponse>();

            CreateMap<Appointment, GetAllAppointmentResponse>();
        }
    }
}
