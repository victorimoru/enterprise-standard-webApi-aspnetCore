using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Infrastructure.Repository
{
    public interface IRepositoryWrapper
    {
        IUserRepository User { get; }
        Task<(string errorMsg, bool transactionStatus)> Complete();
    }
}
