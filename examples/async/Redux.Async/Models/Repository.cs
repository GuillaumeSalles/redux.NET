using Newtonsoft.Json;

namespace Redux.Async.Models
{
    public class Repository
    {
        [JsonProperty(PropertyName = "full_name")]
        public string FullName { get; set; }

        [JsonProperty(PropertyName = "stargazers_count")]
        public int StargazersCount { get; set; }

        [JsonProperty(PropertyName = "forks")]
        public int Forks { get; set; }
    }
}
