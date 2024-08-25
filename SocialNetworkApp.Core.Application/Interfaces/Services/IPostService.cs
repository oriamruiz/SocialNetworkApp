using SocialNetworkApp.Core.Application.ViewModels.Posts;
using SocialNetworkApp.Core.Application.ViewModels.Users;
using SocialNetworkApp.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkApp.Core.Application.Interfaces.Services
{
	public interface IPostService : IGenericService<PostViewModel, SavePostViewModel, Post>
	{
	}
}
