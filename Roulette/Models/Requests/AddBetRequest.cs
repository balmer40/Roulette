using Roulette.Providers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Roulette.Models.Requests
{
    public class AddBetRequest: IValidatableObject
    {
        [Required]
        public Guid GameId { get; set; }

        [Required]
        public Guid CustomerId { get; set; }

        [Required]
        public BetType BetType { get; set; }

        [Range(0, 36)]
        public int Position { get; set; }

        [Range(1, 10000)] //only allow maximum of 10k bet
        public double Amount { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var betHandlerProvider = validationContext.GetService(typeof(IBetHandlerProvider)) as IBetHandlerProvider;
            var betHandler = betHandlerProvider.GetBetHandler(BetType);
            var result = betHandler.ValidatePosition(Position);

            return new[] { result };
        }
    }
}
