using SocialNetworkApp.Core.Application.ViewModels.Comments;
using SocialNetworkApp.Core.Domain.Entities;


namespace SocialNetworkApp.Core.Application.Interfaces.Services
{
	public interface ICommentService : IGenericService<CommentViewModel, SaveCommentViewModel, Comment>
	{
	}
}
