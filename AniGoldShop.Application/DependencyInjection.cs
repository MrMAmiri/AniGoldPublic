using FluentValidation;
using AniGoldShop.Application.Common.Behviours;
using AniGoldShop.Application.Common.Localization;
using AniGoldShop.Application.Common.Localization.Text;
using AniGoldShop.Domain.Interfaces.ExternalServices;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;

namespace AniGoldShop.Application
{
    public static class DependencyInjection
    {
        public static void AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(typeof(DependencyInjection).Assembly);
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            services.AddSingleton(typeof(IStringLocalizer<TextLocalizationResource>), typeof(Localize));
            services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);

        }
    }
}
