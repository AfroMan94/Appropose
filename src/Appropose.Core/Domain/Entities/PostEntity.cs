using System;
using Newtonsoft.Json;

namespace Appropose.Core.Domain.Entities
{
    public class PostEntity : BaseEntity
    {
        [JsonProperty("title")]
        public string Title { get; private set; }

        [JsonProperty("question")]
        public string Question { get; private set; }

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

        [JsonProperty("userId")]
        public string UserId { get; private set; }

        [JsonProperty("retailerName")]
        public string RetailerName { get; private set; }

        [JsonProperty("retailerAddress")]
        public string RetailerAddress { get; private set; }

        private PostEntity(string title, string question, string description, float latitude, float longitude, string userId, string retailerName, string retailerAddress)
        {
            Id = Guid.NewGuid().ToString();
            Title = title;
            Question = question;
            Description = description;
            Latitude = latitude;
            Longitude = longitude;
            CreatedOn = DateTime.Now;
            UserId = userId;
            RetailerName = retailerName;
            RetailerAddress = retailerAddress;
            AngryCount = 0;
        }
        public PostEntity()
        {
        }

        public static PostEntity Create(string title, string question, string description, float latitude, float longitude, string userId, string retailerName, string retailerAddress)
        {
            return new PostEntity(title, question, description, latitude, longitude, userId, retailerName, retailerAddress);
        }

        public void SetImageUrl(string imageUrl)
        {
            ImageUrl = imageUrl;
            ModifiedOn = DateTime.Now;
        }
    }
}
