using System.ComponentModel.DataAnnotations;

namespace HttpClientTutorial.API.HttpClients
{
    public partial class HttpClientSettings : IValidatableObject
    {
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext) => validationContext.Required();
    }
}
