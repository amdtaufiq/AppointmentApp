using AppointmentApp.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace AppointmentApp.Infra.Data.Data
{
    public class AppointmentDBContext : DbContext
    {
        public AppointmentDBContext(DbContextOptions<AppointmentDBContext> opt) : base(opt)
        {
            
        }

        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<Agency> Agencies { get; set; }
        public DbSet<AgencySetting> AgencySettings { get; set; }
        public DbSet<AgencyHoliday> AgencyHolidays { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entity in modelBuilder.Model.GetEntityTypes()
                .Where(x =>
                    x.ClrType.GetProperties().Any(y =>
                        y.CustomAttributes.Any(z =>
                            z.AttributeType == typeof(DatabaseGeneratedAttribute)))))
            {
                foreach (var property in entity.ClrType.GetProperties()
                    .Where(x =>
                        x.PropertyType == typeof(DateTime) && x.CustomAttributes.Any(y =>
                            y.AttributeType == typeof(DatabaseGeneratedAttribute))))
                {
                    modelBuilder
                        .Entity(entity.ClrType)
                        .Property(property.Name)
                        .HasDefaultValueSql("GETUTCDATE()");
                }
            }
        }
    }
}
