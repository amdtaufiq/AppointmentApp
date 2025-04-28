using AppointmentApp.Application.Commands.Appointments;
using AppointmentApp.Infra.Data.Interfaces.UnitOfWork;
using AutoMapper;
using MediatR;

namespace AppointmentApp.Application.Handlers.Appointments
{
    public class UpdateAppointmentHandler : IRequestHandler<UpdateAppointmentCommand, Unit>
    {
        private readonly IUnitOfWork _unitOfWork;

        public UpdateAppointmentHandler(
            IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateAppointmentCommand request, CancellationToken cancellationToken)
        {
            // Retrieve the appointment by ID
            var appointmentEntity = await _unitOfWork.AppointmentQueryRepository.GetByTokenAsync(request.Token);

            if (appointmentEntity == null)
                throw new Exception("Appointment not found.");

            appointmentEntity.IsActive = false;
            // Save the updated appointment to the database
            await _unitOfWork.AppointmentCommandRepository.UpdateAsync(appointmentEntity);
            await _unitOfWork.CommitAsync();

            // Return Unit.Value to signify successful operation with no result
            return Unit.Value;
        }
    }
}
