namespace Ten.Services.BookingDomain.Entities
{
    public class InventoryItem : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public int RemainingCount { get; set; }
        public DateTime ExpirationDate { get; set; }

        public ICollection<Booking> Bookings { get; set; }
    }
}
