namespace Ten.Services.BookingDomain.Entities
{
    public class Booking : BaseEntity
    {
        public Guid BookingReference { get; set; } = Guid.NewGuid();
        public DateTime BookingDate { get; set; } = DateTime.UtcNow;

        public int MemberId { get; set; }
        public Member Member { get; set; }

        public int InventoryItemId { get; set; }
        public InventoryItem InventoryItem { get; set; }
    }
}
