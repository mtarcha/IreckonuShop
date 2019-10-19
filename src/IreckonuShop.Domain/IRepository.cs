using System.Threading;
using System.Threading.Tasks;

namespace IreckonuShop.Domain
{
    public interface IRepository<in TData>
    {
        Task AddOrUpdateAsync(TData data, CancellationToken token);
    }

    public interface IProductsRepository : IRepository<Product>
    { }
}