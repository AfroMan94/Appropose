using System;
using Newtonsoft.Json;

namespace Appropose.Core.Domain.Entities
{
    public class SolutionEntity : BaseEntity
    {
        [JsonProperty("title")]
        public string Title { get; private set; }

        [JsonProperty("description")]
        public string Description { get; private set; }

        [JsonProperty("imageUrl")]
        public string ImageUrl { get; private set; }

        [JsonProperty("likesCount")]
        public int LikesCount { get; private set; }

        [JsonProperty("createdOn")]
        public DateTime CreatedOn { get; private set; }

        [JsonProperty("modifiedOn")]
        public DateTime ModifiedOn { get; private set; }

        [JsonProperty("postId")]
        public string PostId { get; private set; }

        [JsonProperty("userId")]
        public string UserId { get; private set; }

        private SolutionEntity(string title, string description, string userId, string postId)
        {
            Id = Guid.NewGuid().ToString();
            Title = title;
            Description = description;
            CreatedOn = DateTime.Now;
            UserId = userId;
            PostId = postId;
            LikesCount = 0;
        }

        public SolutionEntity()
        {
        }

        public static SolutionEntity Create(string title, string description, string userId, string postId)
        {
            return new SolutionEntity(title, description, userId, postId);
        }

        public void SetImageUrl(string imageUrl)
        {
            ImageUrl = imageUrl;
            ModifiedOn = DateTime.Now;
        }
    }
}
