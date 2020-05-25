using System.Net.Http;
using System.Threading.Tasks;
using ByteDev.GiphyHttpStatus.Web.ViewModels;
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
            var response = await _client.GetStatusImageAsync(code);

            GiphyImage = new GiphyImageViewModel
            {
                Code = response.Code,
                Name = response.Name,
                GiphyImageUrl = response.ImageUrl.ToString()
            };
        }
    }
}
