using System.Collections.Generic;
using Newtonsoft.Json;

namespace WebflowSharp.Entities
{
    public class Field
    {
        public string Key { get; set; }
        public string Value { get; set; }

        public override string ToString()
        {
            return $"{Key}: {Value}";
        }
    }

    public class CreateItem
    {
        [JsonProperty("_id")]
        public string Id { get; set; }

        [JsonProperty("live")]
        public bool Live { get; set; }

        [JsonProperty("fields")]
        public List<Field> Fields { get; set; } = new List<Field>();
    }
}
