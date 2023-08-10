using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Greggs.Products.Services
{
    public static class DecimalExtension
    {

        public static decimal Convert(this decimal value, decimal exchangeRate)
        {
            //2sf
            var conversion = value * exchangeRate;
            return decimal.Round(conversion, 2, MidpointRounding.AwayFromZero);
        }


    }
}
