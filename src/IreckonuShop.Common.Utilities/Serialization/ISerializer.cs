namespace IreckonuShop.Common.Utilities.Serialization
{
    public interface ISerializer<T>
    {
        T Deserialize(string serialization);

        string Serialize(T item);
    }
}