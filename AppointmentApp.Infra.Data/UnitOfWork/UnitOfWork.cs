using AppointmentApp.Domain.Entities;
using AppointmentApp.Infra.Data.Data;
using AppointmentApp.Infra.Data.Interfaces.Commands;
using AppointmentApp.Infra.Data.Interfaces.Queries;
using AppointmentApp.Infra.Data.Interfaces.UnitOfWork;
using AppointmentApp.Infra.Data.Repositories.Commands;
using AppointmentApp.Infra.Data.Repositories.Queries;
using Microsoft.EntityFrameworkCore;

namespace AppointmentApp.Infra.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppointmentDBContext _context;

        public UnitOfWork(AppointmentDBContext context)
        {
            _context = context;

            // Initialize repositories
            AppointmentCommandRepository = new AppointmentCommandRepository(_context);
            AppointmentQueryRepository = new AppointmentQueryRepository(_context);
            AgencyQueryRepository = new AgencyQueryRepository(_context);
        }

        // Command Repositories
        public IAppointmentCommandRepository AppointmentCommandRepository { get; }

        // Query Repositories
        public IAppointmentQueryRepository AppointmentQueryRepository { get; }

        public IAgencyQueryRepository AgencyQueryRepository { get; set; }


        // Rollback changes
        public void Rollback()
        {
            var entries = _context.ChangeTracker.Entries();
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Modified || entry.State == EntityState.Added)
                {
                    entry.State = EntityState.Detached;
                }
            }
        }

        // Commit Changes to Database
        public async Task<int> CommitAsync()
        {
            var entries = _context.ChangeTracker
                .Entries()
                .Where(e => e.Entity is BaseEntity && (
                    e.State == EntityState.Added
                    || e.State == EntityState.Modified));

            foreach (var entityEntry in entries)
            {
                if (entityEntry.State == EntityState.Added)
                {
                    ((BaseEntity)entityEntry.Entity).CreatedAt = DateTime.UtcNow;
                }
                else if (entityEntry.State == EntityState.Modified)
                {
                    ((BaseEntity)entityEntry.Entity).UpdatedAt = DateTime.UtcNow;
                }
            }

            return await _context.SaveChangesAsync();
        }

    }
}
