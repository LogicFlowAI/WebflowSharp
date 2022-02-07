using System.Collections.Generic;
using Newtonsoft.Json;

namespace WebflowSharp.Entities
{
    public class Field
    {
        public string Key { get; set; }
        public object Value { get; set; }
    }

    public class CreateItem
    {
        [JsonProperty("live")]
        public bool Live { get; set; }

        [JsonProperty("fields")]
        public List<Field> Fields { get; } = new List<Field>();
    }
}
