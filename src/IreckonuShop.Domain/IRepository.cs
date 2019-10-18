using System.Threading;
using System.Threading.Tasks;

namespace IreckonuShop.Domain
{
    public interface IRepository<TData>
    {
        Task<TData> CreateAsync(TData data, CancellationToken token);
    }

    public interface IProductsRepository : IRepository<Product>
    { }
}