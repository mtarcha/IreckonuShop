namespace IreckonuShop.FileSystem
{
    public interface ISerializer<T>
    {
        T Deserialize(string serialization);

        string Serialize(T item);
    }
}