using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SocialNetworkApp.Infrastructure.Persistance.Contexts;
using Microsoft.EntityFrameworkCore;
using SocialNetworkApp.Core.Application.Interfaces.Repositories;
using SocialNetworkApp.Infrastructure.Persistance.Repositories;


namespace SocialNetworkApp.Infrastructure.Persistance
{
    public static class ServiceRegistration
    {
        public static void AddPersistanceLayer(this IServiceCollection services, IConfiguration configuration)
        {
            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {

                services.AddDbContext<ApplicationContext>(op => op.UseInMemoryDatabase("DatabaseTest"));


            }
            else
            {
                string stringcon = configuration.GetConnectionString("DefaultConnection");
                services.AddDbContext<ApplicationContext>(op => op.UseSqlServer(stringcon, m => m.MigrationsAssembly(typeof(ApplicationContext).Assembly.FullName)));


            }

            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IPostRepository, PostRepository>();
			services.AddTransient<ICommentRepository, CommentRepository>();
			services.AddTransient<IReplyRepository, ReplyRepository>(); 
            services.AddTransient<IFriendshipRepository, FriendshipRepository>();

        }
    }
}
