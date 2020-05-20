using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Shared.Infrastructure.DatabaseConnection;
using Shared.Infrastructure.LoggingHandler;
using Shared.Infrastructure.Repository.Contracts;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Infrastructure.Repository
{

    public class RepositoryWrapper : IRepositoryWrapper
    {

        private IUserRepository _user;
        private ITokenRepository _token;


        private DataContext _ctx;
        private readonly ILoggerManager loggerManager;

        public RepositoryWrapper(DataContext dataContext, ILoggerManager loggerManager)
        {
            this._ctx = dataContext;
            this.loggerManager = loggerManager;
        }
        public IUserRepository User
        {
            get
            {
                if (_user == null)
                {
                    _user = new UserRepository(_ctx);
                }
                return _user;
            }
        }

        public ITokenRepository Token
        {
            get
            {
                if (_token == null)
                {
                    _token = new TokenRepository(_ctx);
                }
                return _token;
            }
        }

        public async Task<(string errorMsg, bool transactionStatus)> Complete()
        {
            try
            {
                await _ctx.SaveChangesAsync();
                return (null, true);
            }
            catch (DbUpdateException ex)
            {
                loggerManager.LogError(ex.Message);
                return ($"{ex.Message}", false);
            }
            catch (RetryLimitExceededException ex)
            {
                loggerManager.LogError(ex.Message);
                return ($"{ex.Message}", false);
            }
            catch (Exception ex)
            {
                loggerManager.LogError(ex.Message);
                return ($"{ex.Message}", false);
            }

        }
    }
    
}
