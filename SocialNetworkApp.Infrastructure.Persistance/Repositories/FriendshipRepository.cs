using Microsoft.EntityFrameworkCore;
using SocialNetworkApp.Core.Application.Interfaces.Repositories;
using SocialNetworkApp.Core.Domain.Entities;
using SocialNetworkApp.Infrastructure.Persistance.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkApp.Infrastructure.Persistance.Repositories
{
    public class FriendshipRepository : GenericRepository<Friendship>, IFriendshipRepository
    {
        private readonly ApplicationContext _dbContext;

        public FriendshipRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public virtual async Task<Friendship> GetByIdAsyncWithInclude(int id)
        {

            Friendship friendship = await _dbContext.Set<Friendship>()
            .Include(u => u.Friend)
            .Include(u => u.User)
            .FirstOrDefaultAsync(u => u.Id == id);


            return friendship;

        }
    }
}
