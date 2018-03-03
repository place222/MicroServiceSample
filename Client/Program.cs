using IdentityModel.Client;
using System;
using System.Net.Http;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            var disco = DiscoveryClient.GetAsync("http://localhost:5000").Result;
            if (disco.IsError)
            {
                Console.WriteLine(disco.Error);
            }
            var tokenClient = new TokenClient(disco.TokenEndpoint, "client", "secret");
            var tokenResponse = tokenClient.RequestClientCredentialsAsync("api1").Result;
            if (tokenResponse.IsError)
            {
                Console.WriteLine(tokenResponse.Error);
                return;
            }
            Console.WriteLine($"Token:{tokenResponse.Json}");

            var client = new HttpClient();
            client.SetBearerToken(tokenResponse.AccessToken);
            var response = client.GetAsync("http://localhost:9000/api/values").Result;
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                Console.WriteLine(response.Content.ReadAsStringAsync().Result);
            }
            response = client.GetAsync("http://localhost:9000/api/test").Result;
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                Console.WriteLine(response.Content.ReadAsStringAsync().Result);
            }
            response = client.GetAsync("http://localhost:9000/api/agg").Result;
            if (!response.IsSuccessStatusCode)
            {
                Console.WriteLine(response.StatusCode);
            }
            else
            {
                Console.WriteLine(response.Content.ReadAsStringAsync().Result);
            }
            Console.ReadKey();
        }
    }
}
