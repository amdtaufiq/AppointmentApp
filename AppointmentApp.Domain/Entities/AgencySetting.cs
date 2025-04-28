namespace AppointmentApp.Domain.Entities
{
    public class AgencySetting : BaseEntity
    {
        public long AgencyId { get; set; }
        public virtual Agency Agency { get; set; }
        public int MaxAppointmentsPerDay { get; set; }
    }
}
