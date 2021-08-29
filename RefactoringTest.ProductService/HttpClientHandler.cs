using System.Net.Http;

namespace RefactoringTest.ProductService
{
    public interface IHttpClientHandler
    {
        string ReadAsStringAsync(string externalPath);
    }
    public class HttpClientHandler : IHttpClientHandler
    {
        public string ReadAsStringAsync(string externalPath)
        {
            var client = new HttpClient();
            return client.GetAsync(externalPath).Result.Content.ReadAsStringAsync().Result;
        }
    }
}
