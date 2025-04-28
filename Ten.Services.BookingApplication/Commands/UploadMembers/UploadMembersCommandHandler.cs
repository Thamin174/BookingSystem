using System.Globalization;
using MediatR;
using Ten.Services.BookingApplication.Interfaces;
using Ten.Services.BookingDomain.Entities;

namespace Ten.Services.BookingApplication.Commands.UploadMembers
{
    public class UploadMembersCommandHandler : IRequestHandler<UploadMembersCommand, bool>
    {
        private readonly IRepository<Member> _memberRepository;

        public UploadMembersCommandHandler(IRepository<Member> memberRepository)
        {
            _memberRepository = memberRepository;
        }

        public async Task<bool> Handle(UploadMembersCommand request, CancellationToken cancellationToken)
        {
            if (request.File == null || request.File.Length <= 0)
                throw new ArgumentException("Invalid file upload");

            using var streamReader = new StreamReader(request.File.OpenReadStream());
            var lineNumber = 0;

            while (!streamReader.EndOfStream)
            {
                var line = await streamReader.ReadLineAsync();
                if (lineNumber++ == 0) continue;

                var values = line.Split(',');

                var member = new Member
                {
                    Name = values[0].Trim(),
                    Surname = values[1].Trim(),
                    BookingCount = int.Parse(values[2]),
                    DateJoined = DateTime.Parse(values[3], CultureInfo.InvariantCulture)
                };

                await _memberRepository.AddAsync(member);
            }

            await _memberRepository.SaveChangesAsync();
            return true;
        }
    }
}
