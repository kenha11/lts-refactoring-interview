namespace RefactoringTest.ProductService
{
    public interface IProductValidator
    {
        bool IsValidProduct(string productName, string productDescription, decimal price, int quantity, string brandName);
    }

    public class ProductValidator : IProductValidator
    {
        public bool IsValidProduct(string productName, string productDescription, decimal price, int quantity, string brandName)
        {
            if (string.IsNullOrEmpty(productName) || string.IsNullOrEmpty(productDescription) || price <= 0 || quantity <= 0 || string.IsNullOrEmpty(brandName))
                return false;

            return true;
        }
    }
}
