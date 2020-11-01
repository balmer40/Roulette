using Roulette.Providers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Roulette.Constants;

namespace Roulette.Models.Requests
{
    public class AddBetRequest: IValidatableObject
    {
        [Required]
        [RegularExpression(RegularExpressions.NonEmptyGuid)]
        public Guid CustomerId { get; set; }

        [Required]
        [EnumDataType(typeof(BetType))]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public BetType BetType { get; set; }

        [Range(Ranges.MinimumPosition, Ranges.MaximumPosition)]
        public int? Position { get; set; }

        [Range(Ranges.MinimumAmount, Ranges.MaximumAmount)]
        [RegularExpression(RegularExpressions.Amount, ErrorMessage = "Amount can only have two decimal places")]
        public double Amount { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var betTypeHandlerProvider = validationContext.GetService(typeof(IBetTypeHandlerProvider)) as IBetTypeHandlerProvider;
            if(betTypeHandlerProvider == null)
            {
                throw new NullReferenceException("BetHandlerProvider cannot be null");
            }
            var betTypeHandler = betTypeHandlerProvider.GetBetTypeHandler(BetType);
            var result = betTypeHandler.ValidatePosition(Position);

            return new[] { result };
        }
    }
}
