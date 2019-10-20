namespace IreckonuShop.PersistenceLayer.RelationalDb.Entities
{
    public class ColorEntity
    {
        public long Argb { get; set; }
        public byte Alpha { get; set; }
        public byte Red { get; set; }
        public byte Green { get; set; }
        public byte Blue { get; set; }
    }
}