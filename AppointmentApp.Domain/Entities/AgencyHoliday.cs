namespace AppointmentApp.Domain.Entities
{
    public class AgencyHoliday : BaseEntity
    {
        public long AgencyId { get; set; }
        public DateTime? HolidayDate { get; set; }
        public int? DayOfWeek { get; set; }
        public string Description { get; set; }
        public virtual Agency Agency { get; set; }
    }
}
