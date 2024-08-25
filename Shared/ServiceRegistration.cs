using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SocialNetworkApp.Core.Application.Interfaces.Services;
using SocialNetworkApp.Core.Application.Services;
using SocialNetworkApp.Core.Domain.Settings;
using SocialNetworkApp.Infrastructure.Shared.Services;


namespace SocialNetworkApp.Infrastructure.Shared
{
    public static class ServiceRegistration
    {
        public static void AddSharedInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {

            services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
            services.AddTransient<IEmailService, EmailService>();
		}
    }
}
