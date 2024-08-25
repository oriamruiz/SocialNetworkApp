using Microsoft.EntityFrameworkCore;
using SocialNetworkApp.Core.Application.Helpers.Encryptions;
using SocialNetworkApp.Core.Application.Interfaces.Repositories;
using SocialNetworkApp.Core.Application.ViewModels.Users;
using SocialNetworkApp.Core.Domain.Entities;
using SocialNetworkApp.Infrastructure.Persistance.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkApp.Infrastructure.Persistance.Repositories
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly ApplicationContext _dbContext;

        public UserRepository(ApplicationContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task UpdateAsyncWithoutEncryption(User user)
        {
            _dbContext.Entry(user).State = EntityState.Modified;
            await _dbContext.SaveChangesAsync();
        }

        public override async Task<User> AddAsync(User user)
        {
            user.Password = PasswordEncryption.ComputeSha256Hash(user.Password);
            await _dbContext.Set<User>().AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return user;
            
        }

        public override async Task UpdateAsync(User user, int id)
        {
            user.Password = PasswordEncryption.ComputeSha256Hash(user.Password);
            User entry = await _dbContext.Set<User>().FindAsync(id);
            _dbContext.Entry(entry).CurrentValues.SetValues(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<User> LoginAsync(LoginUserViewModel LoginVm)
        {
            string passwordEncrypt = PasswordEncryption.ComputeSha256Hash(LoginVm.Password);


            User user = await _dbContext.Set<User>()
                .Include(u=> u.Posts)
                    .ThenInclude(p=>p.Comments)
                        .ThenInclude(c=> c.User)
                .Include(u => u.Posts)
                    .ThenInclude(p => p.Comments)
                        .ThenInclude(c => c.Replies)
                .Include(u => u.Friends)
                .FirstOrDefaultAsync(u => u.Username == LoginVm.Username && u.Password == passwordEncrypt);

            
            return user;
        }

		public virtual async Task<User> GetByIdAsyncWithInclude(int id)
		{

			User user = await _dbContext.Set<User>()
			.Include(u => u.Posts)
				.ThenInclude(p => p.Comments)
					.ThenInclude(c => c.User)
			.Include(u => u.Posts)
				.ThenInclude(p => p.Comments)
					.ThenInclude(c => c.Replies)
			.Include(u => u.Friends)
			    .ThenInclude(p => p.Friend)
                    .ThenInclude(p=>p.Posts)
                        .ThenInclude(p=>p.Comments)
                            .ThenInclude(p=>p.Replies)
            .Include(u => u.Friends)
                .ThenInclude(p => p.Friend)
                    .ThenInclude(p => p.Posts)
                        .ThenInclude(p => p.Comments)
                            .ThenInclude(p => p.User)

            .FirstOrDefaultAsync(u => u.Id == id);


			return user;

		}

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _dbContext.Set<User>().FirstOrDefaultAsync(u => u.Username == username);

        }

    }
}
