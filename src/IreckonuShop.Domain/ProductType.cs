using System;

namespace IreckonuShop.Domain
{
    public class ProductType
    {
        public ProductType(string artikelCode, string colorCode)
        {
            if (string.IsNullOrEmpty(artikelCode) || string.IsNullOrEmpty(artikelCode))
            {
                throw new ArgumentException($"Product {nameof(artikelCode)} is not initialized.");
            }

            if (string.IsNullOrEmpty(colorCode) || string.IsNullOrEmpty(colorCode))
            {
                throw new ArgumentException($"Product {nameof(colorCode)} is not initialized.");
            }

            ArtikelCode = artikelCode;
            ColorCode = colorCode;
        }

        public string ArtikelCode { get; }
        public string ColorCode { get; }
    }
}