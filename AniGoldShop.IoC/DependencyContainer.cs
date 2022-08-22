using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AniGoldShop.Inferastructure.Data.DataContext;
using Microsoft.EntityFrameworkCore;
using AniGoldShop.Application;
using AniGoldShop.Domain.Interfaces;
using AniGoldShop.Inferastructure.Data.Repository;
using AniGoldShop.Application.Common.Exceptions;
using Jose;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
using Microsoft.IdentityModel.Tokens;
using AniGoldShop.Application.Common.ViewModel;
using AniGoldShop.Domain.Interfaces.ExternalServices;
using AniGoldShop.Infrastructure.SMSService.GhasedakSMSService;
using AniGoldShop.Inferastructure.PriceAPIService;

namespace AniGoldShop.IoC
{
    public static class DependencyContainer
    {
        
        public static readonly string _corsPolicyName = "corsPolicy";
        

        public static void AddRegisterServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<AnigoldContext>(
                 opt => opt.UseSqlServer(configuration.GetConnectionString("SqlServiceConnection"))
             );
            services.AddApplicationServices();


            // add sms service
            services.AddSingleton<ISmsService, WewiSMS>(f => new WewiSMS
                (
                    usern: configuration.GetSection("SMSService").GetSection("Username").Value,
                    pass: configuration.GetSection("SMSService").GetSection("Password").Value,
                    pattern: configuration.GetSection("SMSService").GetSection("Pattern").Value
                )
            );

            // add sms service
            services.AddSingleton<IPriceAPIService, PriceAPIService>();
            services.AddApiVersioningExtension();

            services.AddScoped(typeof(IRepository<,>), typeof(Repository<,>));
        }

        public static void AddJwtAuthenticationService(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddCors(opt =>
            {
                opt.AddPolicy(
                    name: _corsPolicyName,
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                                 .AllowAnyHeader()
                                 .AllowAnyMethod();
                    });
            });


            // get jwtSettings from appsettings.json
            IConfigurationSection jwtSettingSection = Configuration.GetSection("JWTSettings");
            services.Configure<JWTSettings>(jwtSettingSection);
            JWTSettings jwtSettings = jwtSettingSection.Get<JWTSettings>();

            // encoding jwt secret key
            var secretKey = Encoding.UTF8.GetBytes(jwtSettings.SecretKey);

            // add jwt authentication service
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                };
            });

            services.AddMvc()
                .AddNewtonsoftJson(op => op.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
        }

    }
}
