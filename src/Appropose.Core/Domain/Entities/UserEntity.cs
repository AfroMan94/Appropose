using Newtonsoft.Json;

namespace Appropose.Core.Domain.Entities
{
    public class UserEntity : BaseEntity
    {
        [JsonProperty("login")]
        public string Login { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("post")]
        public string[] Posts { get; set; }

    }
}
