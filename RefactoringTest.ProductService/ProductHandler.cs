using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RefactoringTest.ProductService
{

    public interface IProductHandler
    {
        void AddProduct(int id, string productName, string productDescription, decimal price, int quantity, string brandName);
        int GetNextAvailableId(IEnumerable<int> allAvailableIds, string usedIdsFileName);
    }
    public class ProductHandler : IProductHandler
    {
        public void AddProduct(int id, string productName, string productDescription, decimal price, int quantity, string brandName)
        {
            ProductRepository.AddProduct(id, productName, productDescription, price, quantity, brandName);
        }

        public int GetNextAvailableId(IEnumerable<int> allAvailableIds, string usedIdsFileName)
        {
            var usedIds = File.ReadAllLines(usedIdsFileName).Select(int.Parse);
            return allAvailableIds.FirstOrDefault(x => usedIds.All(i => i != x));
        }
    }
}
