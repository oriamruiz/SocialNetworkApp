using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Infrastructure;
using SocialNetworkApp.Core.Application.Helpers.Sessions;
using SocialNetworkApp.Core.Application.Interfaces.Repositories;
using SocialNetworkApp.Core.Application.Interfaces.Services;
using SocialNetworkApp.Core.Application.ViewModels.Friendships;
using SocialNetworkApp.Core.Application.ViewModels.Posts;
using SocialNetworkApp.Core.Application.ViewModels.Users;
using SocialNetworkApp.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkApp.Core.Application.Services
{
	public class PostService : GenericService<PostViewModel, SavePostViewModel, Post> ,IPostService
	{
		private readonly IPostRepository _postRepository;
		private readonly IHttpContextAccessor _httpContext;
		private readonly UserViewModel _userViewModel;
        private readonly IMapper _mapper;

        public PostService(IPostRepository repository, IHttpContextAccessor httpContext, IMapper mapper) : base(repository,mapper)
		{
			_postRepository = repository;
			_httpContext = httpContext;
			_userViewModel = httpContext.HttpContext.Session.Get<UserViewModel>("user");
			_mapper = mapper;

        }

		public override async Task<List<PostViewModel>> GetAllViewModel()
		{
			List<Post> postlist = await _postRepository.GetAllWithInclude();

			List<int> friendsid = _userViewModel.Friends.Select(f => f.FriendId).ToList();

			List<Post> friendsposts = postlist.Where(p => friendsid.Contains(p.UserId)).ToList();


            List<PostViewModel> postsvm = _mapper.Map<List<PostViewModel>>(friendsposts);

            return postsvm;

		}
		public override async Task<SavePostViewModel> CreateViewModel(SavePostViewModel vm)
		{
			

			vm.UserId = _userViewModel.Id;
			vm.PostingDate = DateTime.Now;

			return await base.CreateViewModel(vm);

		}
		

		public override async Task UpdateViewModel(SavePostViewModel vm, int id)
		{
			Post post = await _postRepository.GetByIdAsync(vm.Id);

			post.Id = vm.Id;
			post.Text = vm.Text;
			post.ImgUrl = vm.ImgUrl;
			post.VideoUrl= vm.VideoUrl;
			post.LastModified = DateTime.Now;
			await _postRepository.UpdateAsync(post, id);
		}

		
	}
}
