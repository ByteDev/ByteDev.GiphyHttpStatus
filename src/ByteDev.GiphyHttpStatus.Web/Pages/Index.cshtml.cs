using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ByteDev.GiphyHttpStatus.Web.ViewModels;
using ByteDev.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace ByteDev.GiphyHttpStatus.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly GiphyHttpStatusClient _client;

        public GiphyImageViewModel GiphyImage { get; set; }

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;

            _client = new GiphyHttpStatusClient(new HttpClient(), ApiKeys.Valid);
        }

        public async Task OnGet(int code = 100)
        {
            var httpStatusCode = HttpStatusCode.CreateFromCode(code);

            var response = await _client.GetStatusImageAsync(code);

            var img = response.Gifs.First();

            GiphyImage = new GiphyImageViewModel
            {
                Code = httpStatusCode.Code,
                Name = httpStatusCode.Name,
                GiphyImageUrl = img.Images.Original.Url.ToString()
            };
        }
    }
}
