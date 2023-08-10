using System;

namespace Greggs.Products.Api.ExtensionMethods
{
    public static class StringValidation
    {
        public static bool ValidateTenent(this string tennent)
        {
            if (string.IsNullOrEmpty(tennent)) throw new System.ArgumentNullException($"{nameof(tennent)} cannot be null.");

            //hc code time
            if (tennent.ToLower() == "fr")
            {
                return true;
            }
            else if (tennent.ToLower() == "uk")
            {
                return true;

            }


            throw new ArgumentOutOfRangeException($"{nameof(tennent)} is not a known tennant.");

        }
    }
}
