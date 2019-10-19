using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using IreckonuShop.Domain;
using IreckonuShop.PersistenceLayer.Relational.Entities;
using Microsoft.EntityFrameworkCore;

namespace IreckonuShop.PersistenceLayer.Relational
{
    public class SqlProductsRepository : IProductsRepository
    {
        private readonly IreckonuShopDbContext _dbContext;

        public SqlProductsRepository(IreckonuShopDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbContext.Database.EnsureCreated();
        }

        public async Task AddOrUpdateAsync(Product data, CancellationToken token)
        {
            var type = await GetOrCreateEntity(data.Type, token);
            var deliveryRange = await GetOrCreateEntity(data.Delivery, token);
            var targetClient = await GetOrCreateEntity(data.Q1, token);
            var color = await GetOrCreateEntity(data.Color, token);

            var entity = await _dbContext.Products.SingleOrDefaultAsync(x => x.Key == data.Key, token);

            if (entity == null)
            {
                entity = new ProductEntity
                {
                    Key = data.Key,
                    Type = type,
                    Description = data.Description,
                    Price = data.Price,
                    Discount = data.Discount,
                    Delivery = deliveryRange,
                    TargetClient = targetClient,
                    Size = data.Size,
                    Color = color,
                };

                await _dbContext.Products.AddAsync(entity, token);
            }
            else
            {
                entity.Type = type;
                entity.Description = data.Description;
                entity.Price = data.Price;
                entity.Discount = data.Discount;
                entity.Delivery = deliveryRange;
                entity.TargetClient = targetClient;
                entity.Size = data.Size;
                entity.Color = color;
            }
            
            await _dbContext.SaveChangesAsync(token);
        }

        private async Task<ProductTypeEntity> GetOrCreateEntity(ProductType type, CancellationToken token)
        {
            var entity = await _dbContext.Set<ProductTypeEntity>()
                             .SingleOrDefaultAsync(x => x.ArtikelCode == type.ArtikelCode, token);

            if (entity == null)
            {
                entity = new ProductTypeEntity
                {
                    ArtikelCode = type.ArtikelCode,
                    ColorCode = type.ColorCode,
                };
            }

            return entity;
        }

        private async Task<ColorEntity> GetOrCreateEntity(Color color, CancellationToken token)
        {
            var entity = await _dbContext.Set<ColorEntity>()
                .SingleOrDefaultAsync(x => x.Argb == color.ToArgb(), token);

            if (entity == null)
            {
                entity = new ColorEntity
                {
                    Argb = color.ToArgb(),
                    Alpha = color.A,
                    Red = color.R,
                    Green = color.G,
                    Blue = color.B
                };
            }

            return entity;
        }

        private async Task<DeliveryRangeEntity> GetOrCreateEntity(DeliveryRange deliveryRange, CancellationToken token)
        {
            var min = deliveryRange.Min.Ticks;
            var max = deliveryRange.Max.Ticks;
            var entity = await _dbContext.Set<DeliveryRangeEntity>()
                .SingleOrDefaultAsync(x => x.Min == min && x.Max == max, token);

            if (entity == null)
            {
                entity = new DeliveryRangeEntity
                {
                    Min = deliveryRange.Min.Ticks,
                    Max = deliveryRange.Max.Ticks
                };
            }

            return entity;
        }

        private async Task<TargetClientEntity> GetOrCreateEntity(TargetClient targetClient, CancellationToken token)
        {
            var entity = await _dbContext.Set<TargetClientEntity>()
                .SingleOrDefaultAsync(x => x.TargetClient == targetClient.ToString(), token);

            if (entity == null)
            {
                entity = new TargetClientEntity
                {
                    TargetClient = targetClient.ToString()
                };
            }

            return entity;
        }
    }
}