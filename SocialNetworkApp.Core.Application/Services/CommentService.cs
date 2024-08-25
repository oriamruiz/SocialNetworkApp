using AutoMapper;
using Microsoft.AspNetCore.Http;
using SocialNetworkApp.Core.Application.Helpers.Sessions;
using SocialNetworkApp.Core.Application.Interfaces.Repositories;
using SocialNetworkApp.Core.Application.Interfaces.Services;
using SocialNetworkApp.Core.Application.ViewModels.Comments;
using SocialNetworkApp.Core.Application.ViewModels.Posts;
using SocialNetworkApp.Core.Application.ViewModels.Replies;
using SocialNetworkApp.Core.Application.ViewModels.Users;
using SocialNetworkApp.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkApp.Core.Application.Services
{
	public class CommentService : GenericService<CommentViewModel, SaveCommentViewModel, Comment>, ICommentService
	{
		private readonly ICommentRepository _commentrepository;
		private readonly IHttpContextAccessor _httpContext;
		private readonly UserViewModel _userViewModel;
        private readonly IMapper _mapper;

        public CommentService(ICommentRepository repository, IHttpContextAccessor httpContext, IMapper mapper) : base(repository, mapper)
		{
			_commentrepository = repository;
			_httpContext = httpContext;
			_userViewModel = httpContext.HttpContext.Session.Get<UserViewModel>("user");
			_mapper = mapper;
		}

		
        public override async Task<SaveCommentViewModel> CreateViewModel(SaveCommentViewModel vm)
        {
            vm.UserId = _userViewModel.Id;
            vm.PostingDate = DateTime.Now;
            return await base.CreateViewModel(vm);
        }
        
		//public async Task<SaveCommentViewModel> GetByIdViewModel(int id)
		//{
		//	SaveCommentViewModel commentViewModel = new SaveCommentViewModel();

		//	var comment = await _commentrepository.GetByIdAsync(id);

		//	commentViewModel.Id = comment.Id;
		//	commentViewModel.Text = comment.Text;
		//	commentViewModel.PostId = comment.PostId;

		//	return commentViewModel;

		//}

		//public async Task UpdateViewModel(SaveCommentViewModel vm)
		//{
		//	Comment comment = await _commentrepository.GetByIdAsync(vm.Id);

		//	comment.Id = vm.Id;
		//	comment.Text = vm.Text;
		//	await _commentrepository.UpdateAsync(comment,vm.Id);
		//}

		


	}
}
