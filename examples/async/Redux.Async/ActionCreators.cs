using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Redux.Async
{
    public static class ActionCreators
    {
        public static async Task<IAction> SearchRepositories(string searchTerm)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.UserAgent.ParseAdd("Redux.NET async app");

            var jsonResponse = await client.GetStringAsync(new Uri($"https://api.github.com/search/repositories?q={searchTerm}&sort=stars&order=desc"));

            var searchResponse = JsonConvert.DeserializeObject<SearchRepositoriesResponse>(jsonResponse);

            return new SearchRepositoriesAction
            {
                Repositories = searchResponse.Items
            };
        }
    }
}
