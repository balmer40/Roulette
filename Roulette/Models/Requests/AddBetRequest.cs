using Roulette.Providers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Roulette.Models.Requests
{
    public class AddBetRequest: IValidatableObject
    {
        [Required]
        public Guid CustomerId { get; set; }

        [Required]
        [EnumDataType(typeof(BetType))]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public BetType BetType { get; set; }

        [Range(Ranges.MinimumPosition, Ranges.MaximumPosition)]
        public int Position { get; set; }

        [Range(Ranges.MinimumBet, Ranges.MaximumBet)] 
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
