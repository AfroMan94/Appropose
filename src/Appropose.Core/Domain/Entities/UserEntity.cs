using Newtonsoft.Json;

namespace Appropose.Core.Domain.Entities
{
    public class UserEntity : BaseEntity
    {
        [JsonProperty("login")]
        public string Login { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("azureKey")]
        public string AzureKey { get; set; }

    }
}
