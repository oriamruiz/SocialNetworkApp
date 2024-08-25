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
	public class CommentRepository : GenericRepository<Comment>, ICommentRepository
	{
		private readonly ApplicationContext _dbContext;

		public CommentRepository(ApplicationContext dbContext) : base(dbContext)
		{
			_dbContext = dbContext;
		}


	}
}
