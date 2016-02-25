using Newtonsoft.Json;
using System;

namespace Redux.Async
{
    public class ApplicationState
    {
        public Repository[] Repositories { get; set; }

        public bool IsSearching { get; set; }
    }

    public class SearchRepositoriesResponse
    {
        public Repository[] Items { get; set; }
    }

    public class Repository
    {
        [JsonProperty(PropertyName = "full_name")]
        public string FullName { get; set; }

        [JsonProperty(PropertyName = "stargazers_count")]
        public int StargazersCount { get; set; }

        [JsonProperty(PropertyName = "forks")]
        public int Forks { get; set; }

        [JsonProperty(PropertyName = "html_url")]
        public Uri HtmlUrl { get; set; }

        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "language")]
        public string Language { get; set; }
    }
}
