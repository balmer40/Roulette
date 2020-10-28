using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Roulette.Handlers;
using Roulette.Providers;
using Roulette.Repositories;
using Roulette.Services;
using Roulette.Validators;

namespace Roulette
{
    public static class ServiceCollectionExtensions
    {
        public static void AddControllerDependencies(this IServiceCollection services)
        {
            services.TryAddScoped<IRouletteService, RouletteService>();
        }

        public static void AddServiceDependencies(this IServiceCollection services)
        {
            services.TryAddScoped<IGameService, GameService>();
            services.TryAddSingleton<IGameRepository, GameRepositoryStub>();
            services.TryAddSingleton<IBetRepository, BetRepositoryStub>();
        }

        public static void AddSharedDependencies(this IServiceCollection services)
        {
            services.TryAddScoped<IBetTypeValidator, SingleBetTypeValidator>();
            services.TryAddScoped<IBetHandler, SingleBetHandler>();
            services.TryAddScoped(provider => new[]
            {
                provider.GetService<IBetHandler>()
            });
            services.TryAddScoped<IBetHandlerProvider, BetHandlerProvider>();
        }
    }
}
