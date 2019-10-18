using System;

namespace IreckonuShop.Domain
{
    public class DeliveryRange
    {
        public DeliveryRange(TimeSpan min, TimeSpan max)
        {
            if (min > max)
            {
                throw new ArgumentException($"{nameof(min)} must be less or equal then {nameof(max)}");
            }

            Min = min;
            Max = max;
        }

        public TimeSpan Min { get; }
        public TimeSpan Max { get; }
    }
}