using System;
using System.Drawing;
using System.IO.Abstractions.TestingHelpers;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IreckonuShop.Common.Utilities.HashCalculation;
using IreckonuShop.Common.Utilities.Serialization;
using IreckonuShop.Domain;
using NSubstitute;
using NUnit.Framework;

namespace IreckonuShop.PersistenceLayer.FileSystem.UnitTests
{
    [TestFixture]
    public class FileSystemProductsRepositoryTests
    {
        private FileSystemProductsRepository _fileSystemRepository;
        private MockFileSystem _mockFileSystem;
        private ISerializer<Product> _serializer;
        private IHashCalculator _hashCalculator;

        [SetUp]
        public void SetUp()
        {
            _mockFileSystem = new MockFileSystem();
            _mockFileSystem.AddDirectory(@"D:\\Storage");
            _serializer = new JsonSerializer<Product>();
            _hashCalculator = Substitute.For<IHashCalculator>();

            _fileSystemRepository = new FileSystemProductsRepository(
                @"D:\\Storage", 
                _mockFileSystem,
                _serializer,
                _hashCalculator);
        }

        [Test]
        public async Task AddOrUpdateAsync_NewProducts_NewFilesCreated()
        {
            var product1 = new Product(
                "1",
                new ProductType("1/1", "1"),
                "some description 1",
                10,
                0,
                new DeliveryRange(TimeSpan.FromDays(2), TimeSpan.FromDays(10)),
                TargetClient.Girl,
                150,
                Color.Yellow);

            var product2 = new Product(
                "2",
                new ProductType("1/1", "1"),
                "some description 2",
                10,
                0,
                new DeliveryRange(TimeSpan.FromDays(2), TimeSpan.FromDays(10)),
                TargetClient.Girl,
                150,
                Color.Brown);

            _hashCalculator.Calculate(product1.Key).Returns("1234");
            _hashCalculator.Calculate(product2.Key).Returns("123");

            await _fileSystemRepository.AddOrUpdateAsync(product1, CancellationToken.None);
            await _fileSystemRepository.AddOrUpdateAsync(product2, CancellationToken.None);

            var file1 = _mockFileSystem.GetFile(@"D://Storage/1234_0");
            var file2 = _mockFileSystem.GetFile(@"D://Storage/123_0");
            Assert.AreEqual(2, _mockFileSystem.AllFiles.Count(), "Wrong files count");
            Assert.AreEqual(_serializer.Serialize(product1), file1.TextContents, "Wrong data saved");
            Assert.AreEqual(_serializer.Serialize(product2), file2.TextContents, "Wrong data saved");
        }

        [Test]
        public async Task AddOrUpdateAsync_ProductUpdated_FileContentChanged()
        {
            var oldProduct = new Product(
                "1",
                new ProductType("1/1", "1"),
                "some description 1",
                10,
                0,
                new DeliveryRange(TimeSpan.FromDays(2), TimeSpan.FromDays(10)),
                TargetClient.Girl,
                150,
                Color.Yellow);

            var newProduct = new Product(
                "1",
                new ProductType("1/1", "1"),
                "some description 2",
                10,
                0,
                new DeliveryRange(TimeSpan.FromDays(2), TimeSpan.FromDays(10)),
                TargetClient.Girl,
                150,
                Color.Brown);

            _hashCalculator.Calculate(Arg.Any<string>()).Returns("123");

            await _fileSystemRepository.AddOrUpdateAsync(oldProduct, CancellationToken.None);
            await _fileSystemRepository.AddOrUpdateAsync(newProduct, CancellationToken.None);

            var file = _mockFileSystem.GetFile(@"D://Storage/123_0");
            Assert.AreEqual(1, _mockFileSystem.AllFiles.Count(), "Wrong files count");
            Assert.AreEqual(_serializer.Serialize(newProduct), file.TextContents, "Wrong data saved");
        }

        [Test]
        public async Task AddOrUpdateAsync_HashCollision_TwoFilesCreated()
        {
            var product1 = new Product(
                "1",
                new ProductType("1/1", "1"),
                "some description 1",
                10,
                0,
                new DeliveryRange(TimeSpan.FromDays(2), TimeSpan.FromDays(10)),
                TargetClient.Girl,
                150,
                Color.Yellow);

            var product2 = new Product(
                "2",
                new ProductType("1/1", "1"),
                "some description 2",
                10,
                0,
                new DeliveryRange(TimeSpan.FromDays(2), TimeSpan.FromDays(10)),
                TargetClient.Girl,
                150,
                Color.Brown);

            _hashCalculator.Calculate(Arg.Any<string>()).Returns("123");

            await _fileSystemRepository.AddOrUpdateAsync(product1, CancellationToken.None);
            await _fileSystemRepository.AddOrUpdateAsync(product2, CancellationToken.None);

            var file1 = _mockFileSystem.GetFile(@"D://Storage/123_0");
            var file2 = _mockFileSystem.GetFile(@"D://Storage/123_1");
            Assert.AreEqual(2, _mockFileSystem.AllFiles.Count(), "Wrong files count");
            Assert.AreEqual(_serializer.Serialize(product1), file1.TextContents, "Wrong data saved");
            Assert.AreEqual(_serializer.Serialize(product2), file2.TextContents, "Wrong data saved");
        }
    }
}