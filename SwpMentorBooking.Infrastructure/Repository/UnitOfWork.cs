using SwpMentorBooking.Application.Common.Interfaces;
using SwpMentorBooking.Infrastructure.Data;
using SwpMentorBooking.Infrastructure.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwpMentorBooking.Infrastructure.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;
        public IUserRepository User { get; private set; }

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            User = new UserRepository(_context);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
