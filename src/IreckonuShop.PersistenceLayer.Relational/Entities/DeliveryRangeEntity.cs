using System;

namespace IreckonuShop.PersistenceLayer.Relational.Entities
{
    public class DeliveryRangeEntity
    {
        public long Id { get; set; }
        public long Min { get; set; }
        public long Max { get; set; }
    }
}