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
    public class StudentGroupRepository : Repository<StudentGroup>, IStudentGroupRepository
    {
        private readonly ApplicationDbContext _context;

        public StudentGroupRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
