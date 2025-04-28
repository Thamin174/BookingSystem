using System.Globalization;
using CsvHelper.Configuration;
using CsvHelper;
using MediatR;
using Ten.Services.BookingApplication.Interfaces;
using Ten.Services.BookingDomain.Entities;

namespace Ten.Services.BookingApplication.Commands.UploadInventory
{
    public class UploadInventoryCommandHandler : IRequestHandler<UploadInventoryCommand, bool>
    {
        private readonly IRepository<InventoryItem> _inventoryRepository;

        public UploadInventoryCommandHandler(IRepository<InventoryItem> inventoryRepository)
        {
            _inventoryRepository = inventoryRepository;
        }

        public async Task<bool> Handle(UploadInventoryCommand request, CancellationToken cancellationToken)
        {
            if (request.File == null || request.File.Length <= 0)
                throw new ArgumentException("Invalid file upload");

            using var streamReader = new StreamReader(request.File.OpenReadStream());

            // Configure CsvReader to handle quoted fields and map headers
            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                TrimOptions = TrimOptions.Trim,
                IgnoreBlankLines = true,
                BadDataFound = null // Ignore bad data
            };

            using var csv = new CsvReader(streamReader, config);

            // Map CSV headers to model properties (adjust for naming differences)
            csv.Context.RegisterClassMap<InventoryItemMap>();

            var records = csv.GetRecords<InventoryItem>().ToList();

            foreach (var inventory in records)
            {
                await _inventoryRepository.AddAsync(inventory);
            }

            await _inventoryRepository.SaveChangesAsync();
            return true;
        }
    }

    // Class map to handle CSV column names (e.g., "remaining_count" -> "RemainingCount")
    public sealed class InventoryItemMap : ClassMap<InventoryItem>
    {
        public InventoryItemMap()
        {
            Map(m => m.Title).Name("title");
            Map(m => m.Description).Name("description");
            Map(m => m.RemainingCount).Name("remaining_count");
            Map(m => m.ExpirationDate)
                .Name("expiration_date")
                .TypeConverterOption.Format("dd/MM/yyyy"); // Match CSV date format
        }
    }
}
