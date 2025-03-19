using App.Core.Entities;
using Microsoft.EntityFrameworkCore;
using App.Application.Interfaces.Repositories;
using App.Infrastructure.Data;

namespace App.Infrastructure.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        public UserRepository(ApplicationDBContext context) : base(context)
        {
        }
    }
}
