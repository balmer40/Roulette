using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Roulette.Repositories;
using Roulette.Services;

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
            services.TryAddScoped<IGameRepository, GameRepository>();
        }
    }
}
