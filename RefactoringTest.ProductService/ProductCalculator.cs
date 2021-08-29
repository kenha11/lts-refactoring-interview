using System;

namespace RefactoringTest.ProductService
{

    public interface IProductCalculator
    {
        decimal ApplyPromotion(decimal price, string promotion);
    }
    public class ProductCalculator : IProductCalculator
    {
        public decimal ApplyPromotion(decimal price, string promotion)
        {
            if (promotion == ProductConstants.fivePercentOff)
                return price - price * 0.05m;
            else if (promotion == ProductConstants.tenPercentOff)
                return price - price * 0.1m;
            else if (promotion == ProductConstants.twentyPercentOff)
                return price - price * 0.2m;
            else if (!string.IsNullOrEmpty((promotion)))
                throw new ArgumentException("Invalid promotion specified");
            else
                return price;
        }

    }
}
