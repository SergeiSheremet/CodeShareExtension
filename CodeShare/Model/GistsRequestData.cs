using System.Collections.Generic;
using GistsApi;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CodeShare.Model
{
    class GistsRequestData
    {
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }
        [JsonProperty(PropertyName = "public")]
        public bool Public => true;
        [JsonProperty(PropertyName = "files")]
        public IEnumerable<KeyValuePair<string, GistsFileObject>> Files { get; set; }

        public class GistsFileObject
        {
            [JsonProperty(PropertyName = "content")]
            public string Content { get; set; }
        }
    }


}
