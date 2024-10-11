using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SwpMentorBooking.Application.Common.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository User { get; }
        IStudentRepository Student { get; }
        IMentorRepository Mentor { get; }
        IStudentGroupRepository StudentGroup { get; }
        ITopicRepository Topic { get; }
        IWalletRepository Wallet { get; }
        IDbContextTransaction BeginTransaction();
        void Save();
        
    }
}
