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
	public class PostRepository : GenericRepository<Post>, IPostRepository
	{
		private readonly ApplicationContext _dbContext;

		public PostRepository(ApplicationContext dbContext) : base(dbContext)
		{
			_dbContext = dbContext;
		}

		public virtual async Task<List<Post>> GetAllWithInclude()
		{

			List<Post> posts = await _dbContext.Set<Post>()
			.Include(u => u.Comments)
				.ThenInclude(p => p.User)
			.Include(u => u.Comments)
				.ThenInclude(p => p.Replies)
			.Include(p=> p.User)
			.ToListAsync();


			return posts;

		}


	}
}
