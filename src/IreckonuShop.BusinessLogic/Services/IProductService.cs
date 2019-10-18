using System.Threading;
using System.Threading.Tasks;
using IreckonuShop.BusinessLogic.Models;

namespace IreckonuShop.BusinessLogic.Services
{
    public interface IProductService
    {
        Task AddAsync(Product product, CancellationToken token);
    }
}