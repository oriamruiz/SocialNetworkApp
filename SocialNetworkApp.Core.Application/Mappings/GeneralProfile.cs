using AutoMapper;
using SocialNetworkApp.Core.Application.ViewModels.Comments;
using SocialNetworkApp.Core.Application.ViewModels.Friendships;
using SocialNetworkApp.Core.Application.ViewModels.Posts;
using SocialNetworkApp.Core.Application.ViewModels.Replies;
using SocialNetworkApp.Core.Application.ViewModels.Users;
using SocialNetworkApp.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialNetworkApp.Core.Application.Mappings
{
    public class GeneralProfile : Profile
    {
        public GeneralProfile() 
        {
            CreateMap<Comment, CommentViewModel>()
                .ReverseMap();
            
            CreateMap<Comment, SaveCommentViewModel>()
                .ForMember(dest=>dest.RedirectTo, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(dest => dest.Replies, opt => opt.Ignore());

            CreateMap<Reply, ReplyViewModel>()
                .ReverseMap();

            CreateMap<Reply, SaveReplyViewModel>()
                .ForMember(dest => dest.RedirectTo, opt => opt.Ignore())
                .ReverseMap();

            CreateMap<Friendship, FriendshipViewModel>()
                .ReverseMap();

            CreateMap<Friendship, SaveFriendshipViewModel>()
                .ForMember(dest => dest.FriendUsername, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.UserId, opt => opt.Ignore());

            CreateMap<Post, PostViewModel>()
                .ReverseMap();

            CreateMap<Post, SavePostViewModel>()
                .ForMember(dest => dest.ImageFile, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(dest => dest.Comments, opt => opt.Ignore());

            CreateMap<User, UserViewModel>()
               .ReverseMap();

            CreateMap<User, SaveUserViewModel>()
                .ForMember(dest => dest.ImageFile, opt => opt.Ignore())
                .ForMember(dest => dest.ConfirmPassword, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.State, opt => opt.Ignore());


            CreateMap<User, UpdateUserViewModel>()
                .ForMember(dest => dest.ImageFile, opt => opt.Ignore())
                .ForMember(dest => dest.ConfirmPassword, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(dest => dest.Friends, opt => opt.Ignore())
                .ForMember(dest => dest.Posts, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore())
                .ForMember(dest => dest.State, opt => opt.Ignore());

        }
    }
}
