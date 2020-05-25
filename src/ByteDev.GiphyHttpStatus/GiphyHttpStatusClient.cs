using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using ByteDev.Giphy;
using ByteDev.Giphy.Domain;
using ByteDev.Giphy.Request;
using ByteDev.Giphy.Response.Common;
using ByteDev.GiphyHttpStatus.Contracts.Responses;
using ByteDev.Http;

namespace ByteDev.GiphyHttpStatus
{
    public class GiphyHttpStatusClient
    {
        private const int LimitForRandom = 10;

        private readonly HttpClient _httpClient;
        private readonly string _giphyApiKey;

        public GiphyHttpStatusClient(HttpClient httpClient, string giphyApiKey)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _giphyApiKey = giphyApiKey ?? throw new ArgumentNullException(nameof(giphyApiKey));
        }

        public async Task<GetStatusImageResponse> GetStatusImageAsync(int statusCode, GetStatusImageOptions options = null, CancellationToken cancellationToken = default)
        {
            var httpStatusCode = HttpStatusCode.CreateFromCode(statusCode);

            var giphyClient = new GiphyApiClient(_httpClient, new GiphyApiClientSettings());


            var response = await giphyClient.SearchAsync(CreateSearchRequest(httpStatusCode, options), cancellationToken);

            GifResponse gif;

            if (options != null && options.UseRandomFromSet)
            {
                var rnd = new Random();
                int number = rnd.Next(0, LimitForRandom);

                gif = response.Gifs.Skip(number).Take(1).Single();
            }
            else
            {
                gif = response.Gifs.SingleOrDefault();
            }
                

            var imageUrl = GetSmallestImage(gif);

            return new GetStatusImageResponse
            {
                Code = httpStatusCode.Code,
                Name = httpStatusCode.Name,
                ImageUrl = imageUrl
            };
        }


        private SearchRequest CreateSearchRequest(HttpStatusCode httpStatusCode, GetStatusImageOptions options)
        {
            var request = new SearchRequest(_giphyApiKey)
            {
                Query = httpStatusCode.Name,
                Limit = 1,
                Rating = new Rating(RatingType.ParentalGuidance)
            };

            if (options?.UseCodeInQuery != null)
            {
                request.Query = httpStatusCode.Code + " " + request.Query;
            }

            if (options?.QueryPrefix != null)
            {
                request.Query = options.QueryPrefix + " " + request.Query;
            }

            if (options != null && options.UseRandomFromSet)
            {
                request.Limit = LimitForRandom;
            }

            return request;
        }

        private static Uri GetSmallestImage(GifResponse gif)
        {
            if (gif.Images.DownsizedSmall.Url != null) // TODO check client not had bug (this always null?)
                return gif.Images.DownsizedSmall.Url;

            if (gif.Images.Downsized.Url != null)
                return gif.Images.Downsized.Url;

            return gif.Images.Original.Url;
        }
    }

    public class GetStatusImageOptions
    {
        public string QueryPrefix { get; set; }

        public bool UseRandomFromSet { get; set; }

        public bool UseCodeInQuery { get; set; }
    }
}