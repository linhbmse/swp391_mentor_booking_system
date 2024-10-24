using SwpMentorBooking.Application.Common.Interfaces;
using SwpMentorBooking.Domain.Entities;
using SwpMentorBooking.Infrastructure.Data;

namespace SwpMentorBooking.Infrastructure.Repository
{
    public class ResponseRepository : Repository<Response>, IResponseRepository
    {
        private readonly ApplicationDbContext _context;

        public ResponseRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
