using Roulette.Providers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Roulette.Models.Requests
{
    public class AddBetRequest: IValidatableObject
    {
        [Required]
        public Guid GameId { get; }

        [Required]
        public Guid CustomerId { get; }

        [Required]
        public BetType BetType { get; }

        [Range(0, 36)]
        public int Position { get; }

        [Range(0, 10000)] //only allow maximum of 10k bet
        public double Amount { get; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var betHandlerProvider = validationContext.GetService(typeof(IBetHandlerProvider)) as IBetHandlerProvider;
            // TODO check if null?
            var betHandler = betHandlerProvider.GetBetHandler(BetType);
            var result = betHandler.ValidateBetTypeAndPosition(BetType, Position);

            return new[] { result };
        }
    }
}
