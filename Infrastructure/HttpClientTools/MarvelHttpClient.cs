using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace CodeMarvel.Infrastructure
{
    public class MarvelHttpClient
    {
        public HttpClient Client { get; private set; }

        public MarvelHttpClient(HttpClient httpClient,string BaseAddress)
        {
            httpClient.BaseAddress = new Uri(BaseAddress);

        }
    }
}
