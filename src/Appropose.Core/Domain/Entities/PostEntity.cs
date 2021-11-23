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

        [JsonProperty("imageName")]
        public string ImageName { get; private set; }

        [JsonProperty("localization")]
        public string Localization { get; private set; }

        [JsonProperty("angryCount")]
        public int AngryCount { get; private set; }

        [JsonProperty("solutions")]
        public Guid[] Solutions { get; private set; }

        private PostEntity(string title, string description, string localization)
        {
            Id = Guid.NewGuid().ToString();
            Title = title;
            Description = description;
            Localization = localization;
            AngryCount = 0;
        }
        public PostEntity()
        {
        }

        public static PostEntity Create(string title, string description, string localization)
        {
            return new PostEntity(title, description, localization);
        }

        public void SetImageName(string imageName)
        {
            ImageName = imageName;
        }

    }
}
