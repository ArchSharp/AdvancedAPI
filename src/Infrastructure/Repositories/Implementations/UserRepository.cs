using Domain.Entities;
using Infrastructure.Repositories.Interfaces;
using Infrastucture.Data.DbContext;

namespace Infrastructure.Repositories.Implementations
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context)
        {

        }
    }
}
