using System;
using System.Drawing;
using System.Threading;
using System.Threading.Tasks;
using IreckonuShop.Domain;
using IreckonuShop.PersistenceLayer.RelationalDb.Entities;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;

namespace IreckonuShop.PersistenceLayer.RelationalDb.UnitTests
{
    [TestFixture]
    public class SqlProductsRepositoryTests
    {
        private IreckonuShopDbContext _dbContext;
        private SqlProductsRepository _repository;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<IreckonuShopDbContext>()
                .UseInMemoryDatabase(databaseName: "IreckonuShop")
                .Options;

            _dbContext = new IreckonuShopDbContext(options);

            _repository = new SqlProductsRepository(_dbContext);
        }

        [TearDown]
        public void Teardown()
        {
            _dbContext.Dispose();
        }

        [Test]
        public async Task AddOrUpdateAsync_NewProduct_SavedToDB()
        {
            var product = new Product(
                "1",
                new ProductType("1/1", "1"),
                "some description 1",
                10,
                0,
                new DeliveryRange(TimeSpan.FromDays(2), TimeSpan.FromDays(10)),
                TargetClient.Girl,
                150,
                Color.Yellow);

            await _repository.AddOrUpdateAsync(product, CancellationToken.None);

            var savedProduct = await _dbContext.Products.SingleOrDefaultAsync(x => x.Key == product.Key);
            var savedProductType = await _dbContext.Set<ProductTypeEntity>().SingleOrDefaultAsync(x => x.ArtikelCode == product.Type.ArtikelCode);
            var savedDeliveryRange = await _dbContext.Set<DeliveryRangeEntity>().SingleOrDefaultAsync(x => x.Min == product.Delivery.Min.Ticks);
            var savedTargetClient = await _dbContext.Set<TargetClientEntity>().SingleOrDefaultAsync(x => x.TargetClient == product.Q1.ToString());
            var savedColor = await _dbContext.Set<ColorEntity>().SingleOrDefaultAsync(x => x.Argb == product.Color.ToArgb());

            Assert.IsNotNull(savedProduct);
            Assert.IsNotNull(savedProductType);
            Assert.IsNotNull(savedDeliveryRange);
            Assert.IsNotNull(savedTargetClient);
            Assert.IsNotNull(savedColor);
            Assert.AreEqual(product.Description, savedProduct.Description, $"Wrong {nameof(savedProduct.Description)}");
            Assert.AreEqual(product.Price, savedProduct.Price, $"Wrong {nameof(savedProduct.Price)}");
            Assert.AreEqual(product.Discount, savedProduct.Discount, $"Wrong {nameof(savedProduct.Discount)}");
            Assert.AreEqual(product.Size, savedProduct.Size, $"Wrong {nameof(savedProduct.Size)}");
            Assert.AreEqual(product.Type.ColorCode, savedProductType.ColorCode, $"Wrong {nameof(savedProduct.Type.ColorCode)}");
            Assert.AreEqual(product.Delivery.Min.Ticks, savedDeliveryRange.Min, $"Wrong {nameof(savedProduct.Delivery.Min)}");
            Assert.AreEqual(product.Delivery.Max.Ticks, savedDeliveryRange.Max, $"Wrong {nameof(savedProduct.Delivery.Max)}");
        }

        [Test]
        public async Task AddOrUpdateAsync_UpdateProduct_UpdateSavedToDB()
        {
            var oldProduct = new Product(
                "1",
                new ProductType("12", "12"),
                "some description 2",
                10,
                0,
                new DeliveryRange(TimeSpan.FromDays(2), TimeSpan.FromDays(10)),
                TargetClient.Girl,
                150,
                Color.Yellow);

            var newProduct = new Product(
                "1",
                new ProductType("1/1", "1"),
                "some description 1",
                10,
                0,
                new DeliveryRange(TimeSpan.FromDays(2), TimeSpan.FromDays(10)),
                TargetClient.Girl,
                150,
                Color.Yellow);

            await _repository.AddOrUpdateAsync(oldProduct, CancellationToken.None);
            await _repository.AddOrUpdateAsync(newProduct, CancellationToken.None);

            var updatedProduct = await _dbContext.Products.SingleOrDefaultAsync(x => x.Key == newProduct.Key);
            var newProductType = await _dbContext.Set<ProductTypeEntity>().SingleOrDefaultAsync(x => x.ArtikelCode == newProduct.Type.ArtikelCode);
            var savedDeliveryRange = await _dbContext.Set<DeliveryRangeEntity>().SingleOrDefaultAsync(x => x.Min == newProduct.Delivery.Min.Ticks);
            var savedTargetClient = await _dbContext.Set<TargetClientEntity>().SingleOrDefaultAsync(x => x.TargetClient == newProduct.Q1.ToString());
            var savedColor = await _dbContext.Set<ColorEntity>().SingleOrDefaultAsync(x => x.Argb == newProduct.Color.ToArgb());

            Assert.IsNotNull(updatedProduct);
            Assert.IsNotNull(newProductType);
            Assert.IsNotNull(savedDeliveryRange);
            Assert.IsNotNull(savedTargetClient);
            Assert.IsNotNull(savedColor);
            Assert.AreEqual(newProduct.Description, updatedProduct.Description, $"Wrong {nameof(updatedProduct.Description)}");
            Assert.AreEqual(newProduct.Price, updatedProduct.Price, $"Wrong {nameof(updatedProduct.Price)}");
            Assert.AreEqual(newProduct.Discount, updatedProduct.Discount, $"Wrong {nameof(updatedProduct.Discount)}");
            Assert.AreEqual(newProduct.Size, updatedProduct.Size, $"Wrong {nameof(updatedProduct.Size)}");
            Assert.AreEqual(newProduct.Type.ColorCode, newProductType.ColorCode, $"Wrong {nameof(newProductType.ColorCode)}");
            Assert.AreEqual(newProduct.Delivery.Min.Ticks, savedDeliveryRange.Min, $"Wrong {nameof(savedDeliveryRange.Min)}");
            Assert.AreEqual(newProduct.Delivery.Max.Ticks, savedDeliveryRange.Max, $"Wrong {nameof(savedDeliveryRange.Max)}");
        }
    }
}