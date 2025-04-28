namespace AppointmentApp.Domain.Entities
{
    public class Agency : BaseEntity
    {
        public string AgencyName { get; set; }
        public virtual AgencySetting Setting { get; set; }
        public virtual ICollection<AgencyHoliday> Holidays { get; set; }
        public virtual ICollection<Appointment> Appointments { get; set; }
    }
}
