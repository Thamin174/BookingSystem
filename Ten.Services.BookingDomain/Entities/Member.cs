namespace Ten.Services.BookingDomain.Entities
{
    public class Member : BaseEntity
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public int BookingCount { get; set; }
        public DateTime DateJoined { get; set; }

        public ICollection<Booking> Bookings { get; set; }
    }
}
