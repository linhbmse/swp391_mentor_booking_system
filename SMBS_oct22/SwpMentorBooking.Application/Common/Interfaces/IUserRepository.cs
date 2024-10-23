using SwpMentorBooking.Domain.Entities;

namespace SwpMentorBooking.Application.Common.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        User Update(User entity);
    }
}
