using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core
{
    public class BlogEntry
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "content")]
        public string Content { get; set; }
    }
}
