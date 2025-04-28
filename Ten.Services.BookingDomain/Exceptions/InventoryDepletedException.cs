using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ten.Services.BookingDomain.Exceptions
{
    public class InventoryDepletedException : Exception
    {
        public InventoryDepletedException()
            : base("Inventory is depleted, no more items available for booking.")
        {
        }
    }
}
