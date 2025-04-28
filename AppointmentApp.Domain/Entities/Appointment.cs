namespace AppointmentApp.Domain.Entities
{
    public class Appointment : BaseEntity
    {
        public long AgencyId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhoneNumber { get; set; }
        public DateTime AppointmentDate { get; set; }
        public string Token { get; set; }
        public virtual Agency Agency { get; set; }
    }
}
