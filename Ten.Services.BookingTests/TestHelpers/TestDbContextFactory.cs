using Microsoft.EntityFrameworkCore;
using Ten.Services.BookingInfrastructure.Data;

namespace Ten.Services.BookingTests.TestHelpers
{
    public static class TestDbContextFactory
    {
        public static ApplicationDbContext Create()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }
    }
}
