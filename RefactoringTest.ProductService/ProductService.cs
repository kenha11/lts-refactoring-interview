using System;
using System.Collections.Generic;
using System.Linq;

namespace RefactoringTest.ProductService
{
    public class ProductService
    {
        private readonly IEnumerable<int> _allAvailableIds = Enumerable.Range(0, 100000000);
        private const string _usedIdsFileName = "used_ids.txt";
        private const string _brandServiceUrl = "http://brandservice-test.azurewebsites.net?brandName=";

        private readonly IProductValidator _productValidator;
        private readonly IBrandValidator _brandValidator;
        private readonly IProductCalculator _productCalculator;
        private readonly IProductHandler _productHandler;

        //Construtor only for use with the legacy client
        public ProductService()
        {
            _productValidator = new ProductValidator();
            _brandValidator = new BrandValidator(new HttpClientHandler());
            _productCalculator = new ProductCalculator();
            _productHandler = new ProductHandler();
        }

        public ProductService(IProductValidator productValidator, IBrandValidator brandValidator, IProductCalculator productCalculator, IProductHandler productHandler)
        {
            _productValidator = productValidator;
            _brandValidator = brandValidator;
            _productCalculator = productCalculator;
            _productHandler = productHandler;
        }

        public bool AddProduct(Product product)
        {
            return AddProduct(product.ProductName, product.ProductDescription, product.Price, product.Quantity, product.BrandName, product.Promotion);
        }
        
        public bool AddProduct(string productName, string productDescription, decimal price, int quantity, string brandName, string promotion)
        {
            if (!_productValidator.IsValidProduct(productName, productDescription, price, quantity, brandName))
                return false;

            decimal updatedPrice = _productCalculator.ApplyPromotion(price, promotion);

            int id = _productHandler.GetNextAvailableId(_allAvailableIds, _usedIdsFileName);
            
            if(id == 0)
                throw new Exception("No available ids left");

            if (!_brandValidator.IsBrandAllowed(_brandServiceUrl, brandName))
                return false;
            
            _productHandler.AddProduct(id, productName, productDescription, updatedPrice, quantity, brandName);
            
            return true;
        }

    }
}