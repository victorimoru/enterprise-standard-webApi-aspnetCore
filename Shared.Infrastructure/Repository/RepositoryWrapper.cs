using Shared.Infrastructure.DatabaseConnection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Infrastructure.Repository
{
    
        public class RepositoryWrapper : IRepositoryWrapper
        {
          
            private IUserRepository _user;

            private DataContext _ctx;
            public RepositoryWrapper(DataContext dataContext)
            {
                this._ctx = dataContext;
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

            public async Task Complete()
            {
               await  _ctx.SaveChangesAsync();
            }

            Task IRepositoryWrapper.Complete()
            {
                throw new NotImplementedException();
            }
        }
    
}
