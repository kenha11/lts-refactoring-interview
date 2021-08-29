using System.Text;

namespace RefactoringTest.ProductService
{

    public interface IBrandValidator
    {
        bool IsBrandAllowed(string brandServiceUrl, string brandName);
    }

    public class BrandValidator : IBrandValidator
    {
        private readonly IHttpClientHandler _httpClientHandler;
        public BrandValidator(IHttpClientHandler httpClientHandler)
        {
            _httpClientHandler = httpClientHandler;
        }

        public bool IsBrandAllowed(string brandServiceUrl, string brandName)
        {
            var sb = new StringBuilder();
            sb.Append(brandServiceUrl);
            sb.Append(brandName);
            var externalPath = sb.ToString();
            var result = _httpClientHandler.ReadAsStringAsync(externalPath);
            return bool.Parse(result);
        }

    }
}
