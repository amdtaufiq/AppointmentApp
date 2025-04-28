using AppointmentApp.Application.Queries;
using AppointmentApp.Application.Responses.Appointments;
using AppointmentApp.Infra.Data.Interfaces.UnitOfWork;
using AutoMapper;
using MediatR;

namespace AppointmentApp.Application.Handlers.Appointments
{
    public class GetOutstandingAppointmentQueryHandler : IRequestHandler<GetOutstandingAppointmentQuery, List<GetOutstandingAppointmentResponse>>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GetOutstandingAppointmentQueryHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<List<GetOutstandingAppointmentResponse>> Handle(GetOutstandingAppointmentQuery request, CancellationToken cancellationToken)
        {
            var date = request.Outstanding.Date is not null ? request.Outstanding.Date : DateTime.UtcNow.Date;
            var appointments = await _unitOfWork.AppointmentQueryRepository
                .GetOutStandingAppointmentsForDateAsync(
                    request.Outstanding.AgencyId,
                    (DateTime)date);
            return _mapper.Map<List<GetOutstandingAppointmentResponse>>(appointments);
        }
    }
}
