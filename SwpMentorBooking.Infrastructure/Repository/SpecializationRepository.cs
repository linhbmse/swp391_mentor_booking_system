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
    public class SpecializationRepository : Repository<Specialization>, ISpecializationRepository
    {
        private readonly ApplicationDbContext _context;

        public SpecializationRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public Specialization Update(Specialization entity)
        {
            _context.Update(entity);
            return entity;
        }
    }
}
