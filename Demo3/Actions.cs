using Demo3.Context;
using Demo3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo3
{
    public static class Actions
    {
        public static IsajkinContext PublicContext = new IsajkinContext();
        public static List<Product> Products = PublicContext.Products.ToList();

        public static void ChangeProductList(string search, int filtr, int sort)
        {
            Products.Clear();
            Products = PublicContext.Products.ToList();
            string[] words = search.Split(' ');
            foreach (string word in words) 
            {
                Products = Products.Where(w => w.Title.ToLower().Contains(word.ToLower())|| w.Description.ToLower().Contains(word.ToLower())).ToList();
            }
            if (filtr != 0)
            {
                //Products = Products.Where()
            }
        }
    }
}
