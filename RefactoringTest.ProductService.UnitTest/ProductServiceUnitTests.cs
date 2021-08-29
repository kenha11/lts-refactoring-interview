using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace RefactoringTest.ProductService.UnitTest
{
    [TestClass]
    public class ProductServiceUnitTests
    {
        [TestMethod]
        public void AddProduct_ProductPassesAllChecksWithoutPromotion_AddsProduct()
        {
            var product = new Product
            {
                ProductName = "productName",
                ProductDescription = "productDescription",
                Price = 2.5m,
                Quantity = 3,
                BrandName = "brandName"
            };

            int id = 1;

            var productValidatorMock = new Mock<IProductValidator>(MockBehavior.Strict);
            var brandValidatorMock = new Mock<IBrandValidator>(MockBehavior.Strict);
            var productCalculatorMock = new Mock<IProductCalculator>(MockBehavior.Strict);
            var productHandlerMock = new Mock<IProductHandler>(MockBehavior.Strict);

            var productService = new ProductService(productValidatorMock.Object, 
                                                    brandValidatorMock.Object, 
                                                    productCalculatorMock.Object, 
                                                    productHandlerMock.Object);

            productValidatorMock.Setup(s => s.IsValidProduct(It.Is<string>(a => a == product.ProductName),
                                                             It.Is<string>(a => a == product.ProductDescription),
                                                             It.Is<decimal>(a => a == product.Price),
                                                             It.Is<int>(a => a == product.Quantity),
                                                             It.Is<string>(a => a == product.BrandName))).Returns(true);

            productCalculatorMock.Setup(s => s.ApplyPromotion(It.Is<decimal>(a => a == product.Price), 
                                                              It.Is<string>(a => a == product.Promotion))).Returns(product.Price);

            brandValidatorMock.Setup(s => s.IsBrandAllowed(It.IsAny<string>(), 
                                                           It.Is<string>(a => a == product.BrandName))).Returns(true);

            productHandlerMock.Setup(s => s.GetNextAvailableId(It.IsAny<IEnumerable<int>>(), It.IsAny<string>())).Returns(id);
            productHandlerMock.Setup(s => s.AddProduct(It.Is<int>(a => a == id),
                                                       It.Is<string>(a => a == product.ProductName),
                                                       It.Is<string>(a => a == product.ProductDescription),
                                                       It.Is<decimal>(a => a == product.Price),
                                                       It.Is<int>(a => a == product.Quantity),
                                                       It.Is<string>(a => a == product.BrandName)));

            bool result = productService.AddProduct(product);

            Assert.IsTrue(result);

            productValidatorMock.Verify(s => s.IsValidProduct(It.Is<string>(a => a == product.ProductName),
                                                              It.Is<string>(a => a == product.ProductDescription),
                                                              It.Is<decimal>(a => a == product.Price),
                                                              It.Is<int>(a => a == product.Quantity),
                                                              It.Is<string>(a => a == product.BrandName)), Times.Once);

            productCalculatorMock.Verify(s => s.ApplyPromotion(It.Is<decimal>(a => a == product.Price),
                                                               It.Is<string>(a => a == product.Promotion)), Times.Once);

            brandValidatorMock.Verify(s => s.IsBrandAllowed(It.IsAny<string>(),
                                                            It.Is<string>(a => a == product.BrandName)), Times.Once);

            productHandlerMock.Verify(s => s.GetNextAvailableId(It.IsAny<IEnumerable<int>>(), It.IsAny<string>()), Times.Once);

            productHandlerMock.Verify(s => s.AddProduct(It.Is<int>(a => a == id),
                                                        It.Is<string>(a => a == product.ProductName),
                                                        It.Is<string>(a => a == product.ProductDescription),
                                                        It.Is<decimal>(a => a == product.Price),
                                                        It.Is<int>(a => a == product.Quantity),
                                                        It.Is<string>(a => a == product.BrandName)), Times.Once);
        }

        [TestMethod]
        public void AddProduct_ProductPassesAllChecksWithPromotion_AddsProduct()
        {
            var product = new Product
            {
                ProductName = "productName",
                ProductDescription = "productDescription",
                Price = 2m,
                Quantity = 3,
                BrandName = "brandName"
            };

            int id = 1;
            decimal updatedPrice = product.Price - product.Price * 0.2m;

            var productValidatorMock = new Mock<IProductValidator>(MockBehavior.Strict);
            var brandValidatorMock = new Mock<IBrandValidator>(MockBehavior.Strict);
            var productCalculatorMock = new Mock<IProductCalculator>(MockBehavior.Strict);
            var productHandlerMock = new Mock<IProductHandler>(MockBehavior.Strict);

            var productService = new ProductService(productValidatorMock.Object,
                                                    brandValidatorMock.Object,
                                                    productCalculatorMock.Object,
                                                    productHandlerMock.Object);

            productValidatorMock.Setup(s => s.IsValidProduct(It.Is<string>(a => a == product.ProductName),
                                                             It.Is<string>(a => a == product.ProductDescription),
                                                             It.Is<decimal>(a => a == product.Price),
                                                             It.Is<int>(a => a == product.Quantity),
                                                             It.Is<string>(a => a == product.BrandName))).Returns(true);

            productCalculatorMock.Setup(s => s.ApplyPromotion(It.Is<decimal>(a => a == product.Price),
                                                              It.Is<string>(a => a == product.Promotion))).Returns(updatedPrice);

            brandValidatorMock.Setup(s => s.IsBrandAllowed(It.IsAny<string>(),
                                                           It.Is<string>(a => a == product.BrandName))).Returns(true);

            productHandlerMock.Setup(s => s.GetNextAvailableId(It.IsAny<IEnumerable<int>>(), It.IsAny<string>())).Returns(id);
            productHandlerMock.Setup(s => s.AddProduct(It.Is<int>(a => a == id),
                                                       It.Is<string>(a => a == product.ProductName),
                                                       It.Is<string>(a => a == product.ProductDescription),
                                                       It.Is<decimal>(a => a == updatedPrice),
                                                       It.Is<int>(a => a == product.Quantity),
                                                       It.Is<string>(a => a == product.BrandName)));

            bool result = productService.AddProduct(product);

            Assert.IsTrue(result);

            productValidatorMock.Verify(s => s.IsValidProduct(It.Is<string>(a => a == product.ProductName),
                                                              It.Is<string>(a => a == product.ProductDescription),
                                                              It.Is<decimal>(a => a == product.Price),
                                                              It.Is<int>(a => a == product.Quantity),
                                                              It.Is<string>(a => a == product.BrandName)), Times.Once);

            productCalculatorMock.Verify(s => s.ApplyPromotion(It.Is<decimal>(a => a == product.Price),
                                                               It.Is<string>(a => a == product.Promotion)), Times.Once);

            brandValidatorMock.Verify(s => s.IsBrandAllowed(It.IsAny<string>(),
                                                            It.Is<string>(a => a == product.BrandName)), Times.Once);

            productHandlerMock.Verify(s => s.GetNextAvailableId(It.IsAny<IEnumerable<int>>(), It.IsAny<string>()), Times.Once);

            productHandlerMock.Verify(s => s.AddProduct(It.Is<int>(a => a == id),
                                                        It.Is<string>(a => a == product.ProductName),
                                                        It.Is<string>(a => a == product.ProductDescription),
                                                        It.Is<decimal>(a => a == updatedPrice),
                                                        It.Is<int>(a => a == product.Quantity),
                                                        It.Is<string>(a => a == product.BrandName)), Times.Once);
        }
    }
}
