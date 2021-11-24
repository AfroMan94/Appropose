using System;
using Newtonsoft.Json;

namespace Appropose.Core.Domain.Entities
{
    public class UserElementEntity : BaseEntity
    {   
        [JsonProperty("userId")]
        public string UserId { get; set; }
        [JsonProperty("elementId")]
        public string ElementId { get; set; }

        public UserElementEntity(string userId, string postId)
        {
            Id = Guid.NewGuid().ToString();
            UserId = userId;
            ElementId = postId;
        }

        public UserElementEntity()
        {
        
        }

        public static UserElementEntity Create(string userId, string postId)
        {
            return new UserElementEntity(userId, postId);
        }
    }
}
