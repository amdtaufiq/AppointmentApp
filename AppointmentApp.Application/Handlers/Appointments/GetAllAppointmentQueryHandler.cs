using AppointmentApp.Application.Queries;
using AppointmentApp.Application.Responses.Appointments;
using AppointmentApp.Infra.Data.Interfaces.UnitOfWork;
using AutoMapper;
using MediatR;

namespace AppointmentApp.Application.Handlers.Appointments
{
    public class GetAllAppointmentQueryHandler : IRequestHandler<GetAllAppointmentQuery, List<GetAllAppointmentResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetAllAppointmentQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<GetAllAppointmentResponse>> Handle(GetAllAppointmentQuery request, CancellationToken cancellationToken)
        {
            var appointments = await _unitOfWork.AppointmentQueryRepository.GetAllAppointmentsAsync(request.AgencyId);
            return _mapper.Map<List<GetAllAppointmentResponse>>(appointments);
        }
    }
}
