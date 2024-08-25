using AutoMapper;
using Microsoft.AspNetCore.Http;
using SocialNetworkApp.Core.Application.Helpers.Sessions;
using SocialNetworkApp.Core.Application.Interfaces.Repositories;
using SocialNetworkApp.Core.Application.Interfaces.Services;
using SocialNetworkApp.Core.Application.ViewModels.Comments;
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
	public class ReplyService : GenericService<ReplyViewModel, SaveReplyViewModel, Reply>, IReplyService
	{
		private readonly IReplyRepository _replyrepository;
		private readonly IHttpContextAccessor _httpContext;
		private readonly UserViewModel _userViewModel;
        private readonly IMapper _mapper;

        public ReplyService(IReplyRepository repository, IHttpContextAccessor httpContext, IMapper mapper) : base(repository, mapper)
		{
			_replyrepository = repository;
			_httpContext = httpContext;
			_mapper = mapper;
			_userViewModel = httpContext.HttpContext.Session.Get<UserViewModel>("user");
		}

		public async Task<List<ReplyViewModel>> GetAllViewModel()
		{
			return null;

		}
        public override async Task<SaveReplyViewModel> CreateViewModel(SaveReplyViewModel vm)
        {

            vm.UserId = _userViewModel.Id;
            vm.PostingDate = DateTime.Now;

            return await base.CreateViewModel(vm);

        }

  //      public async Task<SaveReplyViewModel> GetByIdViewModel(int id)
		//{
		//	SaveReplyViewModel replyViewModel = new SaveReplyViewModel();

		//	var reply = await _replyrepository.GetByIdAsync(id);

		//	replyViewModel.Id = reply.Id;
		//	replyViewModel.Text = reply.Text;
		//	replyViewModel.CommentId = reply.CommentId;

		//	return replyViewModel;

		//}

		//public async Task UpdateViewModel(SaveReplyViewModel vm)
		//{
		//	Reply reply = await _replyrepository.GetByIdAsync(vm.Id);

		//	reply.Id = vm.Id;
		//	reply.Text = vm.Text;
		//	await _replyrepository.UpdateAsync(reply, vm.Id);
		//}

		
	}
}
