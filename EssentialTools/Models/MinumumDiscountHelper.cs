using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EssentialTools.Models
{
    public class MinumumDiscountHelper : IDiscountHelper
    {
        public decimal ApplyDiscount(decimal totalParam)
        {
            if (totalParam > 100)
            {
                return totalParam * 0.9m;
            }
            else if (totalParam >= 10)
            {
                return totalParam - 5;
            }
            else if (totalParam >= 0)
            {
                return totalParam;
            }
            else
            {
                throw new ArgumentOutOfRangeException();
            }
        }
    }
}