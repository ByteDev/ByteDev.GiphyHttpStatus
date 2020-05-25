using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ByteDev.Giphy;
using ByteDev.Giphy.Domain;
using ByteDev.Giphy.Request;
using ByteDev.Giphy.Response;
using ByteDev.Http;

namespace ByteDev.GiphyHttpStatus
{
    public class GiphyHttpStatusClient
    {
        private readonly HttpClient _httpClient;
        private readonly string _giphyApiKey;

        public GiphyHttpStatusClient(HttpClient httpClient, string giphyApiKey)
        {
            _httpClient = httpClient;
            _giphyApiKey = giphyApiKey;
        }

        public async Task<SearchResponse> GetStatusImageAsync(int statusCode, CancellationToken cancellationToken = default)
        {
            var httpStatusCode = HttpStatusCode.CreateFromCode(statusCode);

            var giphyClient = new GiphyApiClient(_httpClient, new GiphyApiClientSettings());

            SearchResponse response = await giphyClient.SearchAsync(new SearchRequest(_giphyApiKey)
            {
                Query = httpStatusCode.Name,
                Limit = 1,
                Rating = new Rating(RatingType.ParentalGuidance)
            }, cancellationToken);

            return response;
        }
    }
}