using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using IreckonuShop.Domain;

namespace IreckonuShop.BusinessLogic.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductsRepository _repository;
        private readonly ILocalizedStringParser _localizedStringParser;

        public ProductService(IProductsRepository repository, ILocalizedStringParser localizedStringParser)
        {
            _repository = repository;
            _localizedStringParser = localizedStringParser;
        }

        public async Task AddAsync(Models.Product product, CancellationToken token)
        {
            var domainProduct = new Domain.Product(
                product.Key,
                new ProductType(product.ArtikelCode, product.ColorCode), 
                product.Description,
                product.Price,
                product.DiscountPrice,
                _localizedStringParser.ParseDeliveryRange(product.DeliveredIn),
                _localizedStringParser.ParseTargetClient(product.Q1),
                product.Size,
                _localizedStringParser.ParseColor(product.Color));

            await _repository.AddOrUpdateAsync(domainProduct, token);
        }
    }
}