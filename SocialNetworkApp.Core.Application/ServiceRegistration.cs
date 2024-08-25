using Microsoft.Extensions.DependencyInjection;
using SocialNetworkApp.Core.Application.Interfaces.Services;
using SocialNetworkApp.Core.Application.Services;
using System.Reflection;


namespace SocialNetworkApp.Core.Application
{
    public static class ServiceRegistration
    {
        public static void AddApplicationLayer(this IServiceCollection services)
        {

            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IPostService, PostService>();
			services.AddTransient<ICommentService, CommentService>();
			services.AddTransient<IReplyService, ReplyService>();
            services.AddTransient<IFriendshipService, FriendshipService>();
            
        }
    }
}
