using System;
using Newtonsoft.Json;

namespace Appropose.Core.Domain.Entities
{
    public class PostEntity : BaseEntity
    {
        [JsonProperty("title")]
        public string Title { get; private set; }

        [JsonProperty("description")]
        public string Description { get; private set; }

        [JsonProperty("imageUrl")]
        public string ImageUrl { get; private set; }

        [JsonProperty("latitude")]
        public float Latitude { get; private set; }

        [JsonProperty("longitude")]
        public float Longitude { get; private set; }

        [JsonProperty("angryCount")]
        public int AngryCount { get; private set; }

        [JsonProperty("createdOn")]
        public DateTime CreatedOn { get; private set; }

        [JsonProperty("modifiedOn")]
        public DateTime ModifiedOn { get; private set; }

        [JsonProperty("solutions")]
        public string[] Solutions { get; private set; }

        [JsonProperty("userId")]
        public string UserId { get; private set; }

        private PostEntity(string title, string description, float latitude, float longitude, string userId)
        {
            Id = Guid.NewGuid().ToString();
            Title = title;
            Description = description;
            Latitude = latitude;
            Longitude = longitude;
            CreatedOn = DateTime.Now;
            UserId = userId;
            AngryCount = 0;
        }
        public PostEntity()
        {
        }

        public static PostEntity Create(string title, string description, float latitude, float longitude, string userId)
        {
            return new PostEntity(title, description, latitude, longitude, userId);
        }

        public void SetImageUrl(string imageUrl)
        {
            ImageUrl = imageUrl;
            ModifiedOn = DateTime.Now;
        }

    }
}
