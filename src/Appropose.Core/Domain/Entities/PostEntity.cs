using Newtonsoft.Json;
using ToDoList.Core.Domain.Entities;

namespace Appropose.Core.Domain.Entities
{
    public class PostEntity : BaseEntity
    {
        
        [JsonProperty("description")]
        public string Title { get; set; }
    }
}
