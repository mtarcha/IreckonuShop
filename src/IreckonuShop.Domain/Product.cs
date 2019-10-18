using System;
using System.Drawing;

namespace IreckonuShop.Domain
{
    public class Product
    {
        public Product(
            string key, 
            ProductType type, 
            string description, 
            float price, 
            uint discount, 
            DeliveryRange delivery, 
            TargetClient q1,
            uint size, 
            Color color)
        {
            if (string.IsNullOrEmpty(key) || string.IsNullOrEmpty(key))
            {
                throw new ArgumentException($"Product {nameof(key)} is not initialized.");
            }

            if (type == null)
            {
                throw new ArgumentException($"Product {nameof(type)} is not initialized.");
            }

            if (string.IsNullOrEmpty(description) || string.IsNullOrEmpty(description))
            {
                throw new ArgumentException($"Product {nameof(description)} is not initialized.");
            }

            if (price < 0)
            {
                throw new ArgumentException($"Product {nameof(price)} cannot be < 0.");
            }

            if (discount > 100)
            {
                throw new ArgumentException($"Product {nameof(discount)} cannot be > 100%.");
            }

            if (delivery == null)
            {
                throw new ArgumentException($"Product {nameof(delivery)} is not initialized.");
            }

            if (size == 0)
            {
                throw new ArgumentException($"Product {nameof(size)} cannot be 0.");
            }

            Key = key;
            Type = type;
            Description = description;
            Price = price;
            Discount = discount;
            Delivery = delivery;
            Q1 = q1;
            Size = size;
            Color = color;
        }

        public string Key { get; }
        public ProductType Type { get; }
        public string Description { get; }
        public float Price { get; }
        public uint Discount { get; }
        public DeliveryRange Delivery { get; }
        public TargetClient Q1 { get; }
        public uint Size { get; }
        public Color Color { get; }
    }
}