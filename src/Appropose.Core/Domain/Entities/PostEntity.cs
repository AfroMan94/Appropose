using System;
using Newtonsoft.Json;

namespace Appropose.Core.Domain.Entities
{
    public class PostEntity : BaseEntity
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("imagePath")]
        public string ImagePath { get; set; }

        [JsonProperty("localization")]
        public string Localization { get; set; }

        [JsonProperty("angryCount")]
        public int AngryCount { get; set; }

        [JsonProperty("solutions")]
        public Guid[] Solutions { get; set; }

    }

}
