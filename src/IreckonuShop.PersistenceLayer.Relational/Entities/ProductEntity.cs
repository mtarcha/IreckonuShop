using IreckonuShop.Domain;

namespace IreckonuShop.PersistenceLayer.Relational.Entities
{
    public class ProductEntity
    {
        public long Id { get; set; }
        public string Key { get; set; }
        public ProductTypeEntity Type { get; set; }
        public string Description { get; set; }
        public float Price { get; set; }
        public uint Discount { get; set; }
        public DeliveryRangeEntity Delivery { get; set; }
        public TargetClientEntity TargetClient { get; set; }
        public uint Size { get; set; }
        public ColorEntity Color { get; set; }
    }
}