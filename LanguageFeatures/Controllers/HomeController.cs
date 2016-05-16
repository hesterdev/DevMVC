using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LanguageFeatures.Models;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;

namespace LanguageFeatures.Controllers
{
    public class HomeController : Controller
    {
        public string Index()
        {
            return "Navigate to a URL to show an example";
        }
        public ViewResult AutoProperty()
        {
            //Product myProduct = new Product { };

            Product myProduct = new Product()
            {
                ProductID = 100,
                Name = "Kayak",
                Description = "A boat for one person",
                Price = 275M,
                Category = "Watersports",
            };


            // generate the view
            return View("Result", (object)String.Format("Category: {0}", myProduct.Category));
        }

        public ViewResult CreateCollection()
        {
            string[] stringArray = { "apple", "orange", "plum" };
            List<int> intList = new List<int> { 10, 20, 30, 40 };
            Dictionary<string, int> myDict = new Dictionary<string, int>()
            {
                {"apple",10 },
                {"orange",20 },
                {"plum",30 },
            };
            return View("Result", (object)stringArray[1]);
        }

        public ViewResult UseExtension()
        {
            // create and populate shoppingCart
            ShoppingCart cart = new ShoppingCart
            {
                Products = new List<Product>
                {
                    new Product {Name="Kayak",Price=275M },
                    new Product {Name="Lifejacket",Price=48.95M },
                    new Product {Name="Soccer ball",Price=19.50M },
                    new Product {Name="Corner flag",Price=34.95M }
                }
            };

            // get the total value of the products in the cart 
            decimal cartTotal = cart.TotalPrices();

            return View("Result", (object)string.Format("Total: {0:C}", cartTotal));
        }

        public ViewResult UseExtensionEnumerable()
        {
            IEnumerable<Product> products = new ShoppingCart
            {
                Products = new List<Product> {
                    new Product {Name="Kayak",Price=275M },
                    new Product {Name="Lifejacket",Price=48.95M },
                    new Product {Name="Soccer ball",Price=19.50M },
                    new Product {Name="Corner flag",Price=34.95M }
                 }
            };

            // create and populate an array of product objects
            Product[] productArray =
            {
                new Product {Name="Kayak",Price=275M },
                new Product {Name="Lifejacket",Price=48.95M },
                new Product {Name="Soccer ball",Price=19.50M },
                new Product {Name="Corner flag",Price=34.95M }
            };

            // get the total value of the products int the cart
            decimal cartTotal = products.TotalPrices();
            decimal arrayTotal = productArray.TotalPrices();

            return View("Result", (object)String.Format("Cart Total: {0}, Array Total: {1}", cartTotal, arrayTotal));
        }

        public ViewResult UseFilterExtensionMethod()
        {
            IEnumerable<Product> products = new ShoppingCart
            {
                Products = new List<Product>
                {
                    new Product {Name="Kayak",Category="Watersports",Price=275M },
                    new Product {Name="Lifejacket",Category="Watersports",Price=48.95M },
                    new Product {Name="Soccer ball",Category="Soccer",Price=19.50M },
                    new Product {Name="Corner flag",Category="Soccer",Price=34.95M }
                },
            };
            decimal total = 0;

            //Func<Product, bool> categoryFilter = delegate (Product prod)
            //   {
            //       return prod.Category == "Watersports";
            //   };

            #region 写法一
            Func<Product, bool> categoryFilter = prod => prod.Category == "Soccer";


            //foreach(Product prod in products.Filter(categoryFilter))
            //{
            //    total += prod.Price;
            //}
            #endregion

            #region 省略lambda变量定义
            //foreach (var prod in products.Filter(prod => prod.Category == "Soccer"))
            //{
            //    total += prod.Price;
            //}

            foreach (var prod in products.Filter(prod => prod.Category == "Soccer" || prod.Price > 20))
            {
                total += prod.Price;
            }
            #endregion

            //foreach (Product prod in products.FilterByCategory("Soccer"))
            //{
            //    total += prod.Price;
            //}
            return View("Result", (object)String.Format("Total: {0}", total));

        }
        public ViewResult CreateAnonArray()
        {
            var tmp = new { Name = "MVC", Category = "Pattern" };
            var oddsAndEnds = new[]
            {
                new {Name="MVC",Category="Pattern" },
                new {Name="Hat",Category="Clothing",},
                new {Name="Apple",Category="Fruit" },
            };
            StringBuilder result = new StringBuilder();
            foreach (var item in oddsAndEnds)
            {
                result.Append(item.Name).Append(" ");
            }
            return View("Result", (object)result.ToString());
        }
        public ViewResult FindProducts()
        {
            Product[] products =
            {
                 new Product {Name="Kayak",Category="Watersports",Price=275M },
                 new Product {Name="Lifejacket",Category="Watersports",Price=48.95M },
                 new Product {Name="Soccer ball",Category="Soccer",Price=19.50M },
                 new Product {Name="Corner flag",Category="Soccer",Price=34.95M }
            };
            products[3] = new Product { Name = "Stadium", Price = 79600M };
            /*
            // define the array to hold the results
            Product[] foundProducts = new Product[3];
            // Sort the contents of the array
            Array.Sort(products, (item1, item2) =>
            {
                return Comparer<decimal>.Default.Compare(item1.Price, item2.Price);
            });
            // get the first three items in the array as the results 
            Array.Copy(products, foundProducts, 3);
            */

            //Use LINQ to query data
            //var foundProducts = from match in products
            //                    orderby match.Price descending
            //                    select new { match.Name, match.Price };

            //// create the result
            //StringBuilder result = new StringBuilder();
            //int cnt = 0;
            //foreach (var p in foundProducts)
            //{
            //    if (cnt++ < 3)
            //        result.AppendFormat("Price: {0} ", p.Price);
            //}




            var fountProducts = products.OrderByDescending(e => e.Price).Take(3).Select(e => new { e.Name, e.Price });
            StringBuilder result = new StringBuilder();
            foreach(var p in fountProducts)
            {
                result.AppendFormat("Price: {0} ", p.Price);
            }
            return View("Result", (object)result.ToString());

        }

        public ViewResult SumProducts()
        {
            Product[] products =
              {
                 new Product {Name="Kayak",Category="Watersports",Price=275M },
                 new Product {Name="Lifejacket",Category="Watersports",Price=48.95M },
                 new Product {Name="Soccer ball",Category="Soccer",Price=19.50M },
                 new Product {Name="Corner flag",Category="Soccer",Price=34.95M }
            };
            var results = products.Sum(e => e.Price);
            products[2] = new Product { Name = "Stadium", Price = 79500M };
            return View("Result", (object)String.Format("Sum: {0:c}", results));
        }
        /*
        public static Task<long?> GetPageLength()
        {
            HttpClient client = new HttpClient();
            var httpTask=client.GetAsync("http://apress.com");

            // we could do other things here while we are waiting for the HTTP request to complete
            return httpTask.ContinueWith((Task<HttpResponseMessage> antecedent) =>
            {
                return antecedent.Result.Content.Headers.ContentLength;
            });
        }
        */
        public async static Task<long?> GetPageLength()
        {
            HttpClient client = new HttpClient();
            var httpMessage = await client.GetAsync("http://apress.com");
            // we could do other things here while we are waiting for the HTTP request to complete
            return httpMessage.Content.Headers.ContentLength;
        }
    }
}