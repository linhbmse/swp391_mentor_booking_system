using SwpMentorBooking.Application.Common.Interfaces;
using SwpMentorBooking.Domain.Entities;
using SwpMentorBooking.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwpMentorBooking.Infrastructure.Repository
{
    public class MentorScheduleRepository : Repository<MentorSchedule>, IMentorScheduleRepository
    {
        private readonly ApplicationDbContext _context;
        public MentorScheduleRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public MentorSchedule Update(MentorSchedule entity)
        {
            _context.Update(entity);
            return entity;
        }
    }
}
