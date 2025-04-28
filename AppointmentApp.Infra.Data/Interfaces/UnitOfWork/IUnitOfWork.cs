using AppointmentApp.Infra.Data.Interfaces.Commands;
using AppointmentApp.Infra.Data.Interfaces.Queries;

namespace AppointmentApp.Infra.Data.Interfaces.UnitOfWork
{
    public interface IUnitOfWork
    {
        IAppointmentCommandRepository AppointmentCommandRepository { get; }
        IAppointmentQueryRepository AppointmentQueryRepository { get; }
        IAgencyQueryRepository AgencyQueryRepository { get;  }
        Task<int> CommitAsync();
        void Rollback();
    }
}
