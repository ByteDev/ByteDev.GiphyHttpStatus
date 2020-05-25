using System;

namespace ByteDev.GiphyHttpStatus.Contracts.Responses
{
    public class GetStatusImageResponse
    {
        public int Code { get; set; }

        public string Name { get; set; }

        public Uri ImageUrl { get; set; }
    }
}