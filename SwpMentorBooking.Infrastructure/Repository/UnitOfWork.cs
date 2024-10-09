using Microsoft.EntityFrameworkCore.Storage;
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
        public IStudentRepository Student { get; private set; }
        public IMentorRepository Mentor { get; private set; }
        public IStudentGroupRepository StudentGroup { get; private set; }
        public ITopicRepository Topic { get; private set; }
        public IWalletRepository Wallet { get; private set; }
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            User = new UserRepository(_context);
            Student = new StudentRepository(_context);
            Mentor = new MentorRepository(_context);
            StudentGroup = new StudentGroupRepository(_context);
            Topic = new TopicRepository(_context);
            Wallet = new WalletRepository(_context);
        }


        public IDbContextTransaction BeginTransaction()
        {
            return _context.Database.BeginTransaction();
        }
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
