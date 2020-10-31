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
            services.TryAddScoped<IGameService, GameService>();
            services.TryAddScoped<IBetService, BetService>();
        }

        public static void AddServiceDependencies(this IServiceCollection services)
        {
            services.TryAddScoped<ISpinWheelService, SpinWheelService>();
            services.TryAddSingleton<IGameRepository, GameRepositoryStub>();
            services.TryAddSingleton<IBetRepository, BetRepositoryStub>();
        }

        public static void AddBetTypeDependencies(this IServiceCollection services)
        {
            services.TryAddScoped<IBetTypeHandler>(
                provider => 
                    new SingleBetTypeHandler(new SingleBetTypeValidator()));
            services.TryAddScoped<IBetTypeHandler>(
                provider =>
                    new RedBetTypeHandler(new RedBetTypeValidator()));
            services.TryAddScoped<IBetTypeHandler>(
                provider =>
                    new BlackBetTypeHandler(new BlackBetTypeValidator()));
            services.TryAddScoped<IBetTypeHandler>(
                provider =>
                    new ColumnBetTypeHandler(new ColumnBetTypeValidator()));
            services.TryAddScoped<IBetTypeHandler>(
                provider =>
                    new CornerBetTypeHandler(new CornerBetTypeValidator()));
            services.TryAddScoped<IBetTypeHandler>(
                provider =>
                    new SplitHorizontalBetTypeHandler(new SplitHorizontalBetTypeValidator()));
            services.TryAddScoped<IBetTypeHandler>(
                provider =>
                    new SplitVerticalBetTypeHandler(new SplitVerticalBetTypeValidator()));
            services.TryAddScoped(_ => new[]
            {
                (IBetTypeHandler)new SingleBetTypeHandler(new SingleBetTypeValidator()),
                (IBetTypeHandler)new RedBetTypeHandler(new RedBetTypeValidator()),
                (IBetTypeHandler)new BlackBetTypeHandler(new BlackBetTypeValidator()),
                (IBetTypeHandler)new ColumnBetTypeHandler(new ColumnBetTypeValidator()),
                (IBetTypeHandler)new CornerBetTypeHandler(new CornerBetTypeValidator()),
                (IBetTypeHandler)new SplitHorizontalBetTypeHandler(new SplitHorizontalBetTypeValidator()),
                (IBetTypeHandler)new SplitVerticalBetTypeHandler(new SplitVerticalBetTypeValidator()),
            });
            services.TryAddScoped<IBetTypeHandlerProvider, BetTypeHandlerProvider>();
        }
    }
}
