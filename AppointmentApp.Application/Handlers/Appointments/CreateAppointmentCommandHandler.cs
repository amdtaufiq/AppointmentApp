using AppointmentApp.Application.Commands.Appointments;
using AppointmentApp.Application.Helpers;
using AppointmentApp.Application.Responses.Appointments;
using AppointmentApp.Domain.Entities;
using AppointmentApp.Infra.Data.Interfaces.UnitOfWork;
using AutoMapper;
using MediatR;

namespace AppointmentApp.Application.Handlers.Appointments
{
    public class CreateAppointmentCommandHandler : IRequestHandler<CreateAppointmentCommand, CreateAppointmentResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CreateAppointmentCommandHandler(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<CreateAppointmentResponse> Handle(CreateAppointmentCommand request, CancellationToken cancellationToken)
        {
            var isHoliday = await _unitOfWork.AgencyQueryRepository.IsHolidayAsync(
                request.CreateAppointmentRequest.AgencyId, 
                request.CreateAppointmentRequest.AppointmentDate);
            if (isHoliday)
            {
                throw new Exception("Tidak bisa membuat janji temu pada hari libur.");
            }

            var maxAppointments = await _unitOfWork.AgencyQueryRepository.GetMaxAppointmentsPerDayAsync(
                request.CreateAppointmentRequest.AgencyId);
            var currentAppointments = await _unitOfWork.AgencyQueryRepository.GetAppointmentsCountAsync(
                request.CreateAppointmentRequest.AgencyId, 
                request.CreateAppointmentRequest.AppointmentDate);

            if (currentAppointments >= maxAppointments)
            {
                throw new Exception("Kuota janji temu untuk hari ini sudah penuh, silakan pilih hari lain.");
            }

            int existingAppointmentsCount = await _unitOfWork.AppointmentQueryRepository
                .GetAppointmentsCountByDateAsync(request.CreateAppointmentRequest.AppointmentDate);

            int nextNumber = existingAppointmentsCount + 1;

            // Generate token
            string token = TokenGenerator.GenerateToken(request.CreateAppointmentRequest.AppointmentDate, nextNumber);

            var appointment = _mapper.Map<Appointment>(request.CreateAppointmentRequest);

            appointment.Token = token;

            await _unitOfWork.AppointmentCommandRepository.AddAsync(appointment);

            await _unitOfWork.CommitAsync();

            var response = new CreateAppointmentResponse()
            {
                AppointmentDate = request.CreateAppointmentRequest.AppointmentDate,
                Token = token
            };

            return response;
        }
    }
}
