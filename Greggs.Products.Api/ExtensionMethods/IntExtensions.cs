using System;

namespace Greggs.Products.Api.ExtensionMethods
{
    public static class IntValidation
    {

        public static void ValidateNumber(this int number)
        {
            if (number < 0) throw new ArgumentOutOfRangeException("Number cannot be less than 0");
            var numberString = number.ToString();

            var isValid = int.TryParse(numberString, out var validNumber);

            if (!isValid)
            {
                throw new ArgumentException("A piganted number has to be a valid integer.");

            }
        }
    }
}
