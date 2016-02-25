using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Redux.Async
{
    public static class ActionCreators
    {
        public static AsyncActionsCreator<ApplicationState> SearchRepositories(string searchTerm)
        {
            return async (dispatch, getState) =>
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                    return;

                dispatch(new SearchRepositoriesAction());

                var searchResponse = await SearchGithubRepositories(searchTerm);

                dispatch(new ReceiveRepositoriesAction
                {
                    Repositories = searchResponse.Items
                });
            };
        }

        public static async Task<SearchRepositoriesResponse> SearchGithubRepositories(string searchTerm)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.UserAgent.ParseAdd("Redux.NET async app");

            var jsonResponse = await client.GetStringAsync(
                new Uri($"https://api.github.com/search/repositories?q={searchTerm}&sort=stars&order=desc"));

            return JsonConvert.DeserializeObject<SearchRepositoriesResponse>(jsonResponse);
        }
    }
}
