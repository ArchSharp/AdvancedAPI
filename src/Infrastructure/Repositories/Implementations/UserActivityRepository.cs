using Domain.Entities;
using Infrastructure.Data.DbContext;
using Infrastructure.Repositories.Interfaces;

namespace Infrastructure.Repositories.Implementations
{
    public class UserActivityRepository: Repository<UserActivity>, IUserActivityRepository
    {
        public UserActivityRepository(AppDbContext context): base(context)
        {

        }
    }
}
