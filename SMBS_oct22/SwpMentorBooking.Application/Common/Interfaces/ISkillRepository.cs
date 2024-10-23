using SwpMentorBooking.Domain.Entities;

namespace SwpMentorBooking.Application.Common.Interfaces
{
    public interface ISkillRepository : IRepository<Skill>
    {
        Skill Update(Skill entity);
    }
}
