using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.IdentityModel.Tokens;
using MovieApp.Common.AutoMapper;
using MovieApp.Service;
using MovieApp.Service.Helper;
using MovieApp.Service.Services;
using MovieApp.Service.Services.High;
using MovieApp.Service.Services.Low;
using System.Text;

namespace MovieApp.API
{
    public static class ConfigureServices
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthenticateService, AuthenticateService>();
            services.AddScoped<IUserVerificationService, UserVerificationService>();
            services.AddScoped<IUserTokenService, UserTokenService>();

            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRoleService, UserRoleService>();
            services.AddScoped<IUserStatusService, UserStatusService>();
            services.AddScoped<IUserWatchHistoryService, UserWatchHistoryService>();
            services.AddScoped<IUserLikeService, UserLikeService>();

            services.AddScoped<IMovieService, MovieService>();
            services.AddScoped<IMovieCategoryService, MovieCategoryService>();
            services.AddScoped<IMovieRateService, MovieRateService>();
            services.AddScoped<ICategoryService, CategoryService>();

            services.AddScoped<ServiceWrapper>();
            return services;
        }
        public static IServiceCollection AddHelperServices(this IServiceCollection services)
        {
            services.AddScoped<IEmailHelper, EmailHelper>();
            services.AddScoped<ITokenHelper, TokenHelper>();
            services.AddScoped<IPasswordHelper, PasswordHelper>();

            services.AddScoped<HelperWrapper>();
            return services;
        }
        public static IServiceCollection AddSmtpClient(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<SmtpClient>(provider =>
            {
                var smtpClient = new SmtpClient();
                smtpClient.Connect(configuration["SmtpSettings:Host"], int.Parse(configuration["SmtpSettings:Port"]), SecureSocketOptions.StartTls);
                smtpClient.Authenticate(configuration["SmtpSettings:Username"], configuration["SmtpSettings:Password"]);
                return smtpClient;
            });

            return services;
        }
        public static IServiceCollection ConfigureKestrel(this IServiceCollection services)
        {
            services.Configure<KestrelServerOptions>(options =>
            {
                options.Limits.MaxRequestHeadersTotalSize = 64 * 1024; // Ví dụ: 64KB
            });

            return services;
        }
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = configuration["JwtSettings:Issuer"],
                    ValidAudience = configuration["JwtSettings:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JwtSettings:SecretKey"]))
                };
            });

            return services;
        }
        public static IServiceCollection AddAutoMapperConfiguration(this IServiceCollection services)
        {
            services.AddAutoMapper(typeof(MappingProfile));
            return services;
        }

        //===============================================

        public static IServiceCollection ConfigureApiServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApplicationServices();
            services.AddHelperServices();
            services.AddSmtpClient(configuration);
            services.AddAutoMapperConfiguration();
            services.AddJwtAuthentication(configuration);
            services.ConfigureKestrel();

            return services;
        }
    }
}
