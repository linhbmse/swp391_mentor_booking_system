using SwpMentorBooking.Application.Common.Interfaces;
using SwpMentorBooking.Domain.Entities;
using SwpMentorBooking.Infrastructure.Data;

namespace SwpMentorBooking.Infrastructure.Repository
{
    public class SkillRepository : Repository<Skill>, ISkillRepository
    {
        private readonly ApplicationDbContext _context;

        public SkillRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public Skill Update(Skill entity)
        {
            _context.Update(entity);
            return entity;
        }
    }
}
