
using BusinessObject.DTO;
using CloudinaryDotNet;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Repositories.IRepositories;
using Repositories.Repositories;
using Repositories.Repositories.IRepositories;
using Services.IService;
using Services.Service;
using System.Text.Json.Serialization;

namespace RBN_Api.Extensions
{

    public static class ServiceCollectionExtensions
    {
	
		public static IServiceCollection Register(this IServiceCollection services)
        {
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });
		

			// Configure AutoMapper
			services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Register repositories here
            
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IEventRepository, EventRepository>();
            services.AddScoped<IEventImgRepository, EventImgRepository>();
            
            services.AddScoped<IFeedbackRepository, FeedbackRepository>();
            services.AddScoped<IResponseRepository, ResponseRepository>();


            services.AddScoped<IBookingRepository, BookingRepository>();

            // Register services here
            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IUserService, UserService>();
            services.AddTransient<ISendMailService, SendMailService>();
            services.AddScoped<IEventService, EventService>();
			services.AddScoped<IEventImgService, EventImgService>();
			services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IFeedbackService, FeedbackService>();
            services.AddScoped<IResponseService, ResponseService>();
          
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IBookingService, BookingService>();
            services.AddScoped<IPromotionService, PromotionService>();

            return services;
        }
    }
}
