using System;
using System.IO.Abstractions;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IreckonuShop.Common.Utilities.HashCalculation;
using IreckonuShop.Common.Utilities.Serialization;
using IreckonuShop.Domain;

namespace IreckonuShop.PersistenceLayer.FileSystem
{
    public class FileSystemProductsRepository : IProductsRepository
    {
        private readonly string _storageFolder;
        private readonly IFileSystem _fileSystem;
        private readonly ISerializer<Product> _serializer;
        private readonly IHashCalculator _hashCalculator;

        public FileSystemProductsRepository(
            string storageFolder, 
            IFileSystem fileSystem, 
            ISerializer<Product> serializer,
            IHashCalculator hashCalculator)
        {
            _storageFolder = storageFolder ?? throw new ArgumentNullException(nameof(storageFolder));
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
            _serializer = serializer ?? throw new ArgumentNullException(nameof(serializer));
            _hashCalculator = hashCalculator ?? throw new ArgumentNullException(nameof(hashCalculator));
        }

        public Task AddOrUpdateAsync(Product data, CancellationToken token)
        {
            return Task.Run(() =>
            {
                var storage = _fileSystem.DirectoryInfo.FromDirectoryName(_storageFolder);

                var partialName = _hashCalculator.Calculate(data.Key);
                var filesInDir = storage.GetFiles(partialName + "_*").ToArray();
                foreach (var foundFile in filesInDir)
                {
                    token.ThrowIfCancellationRequested();
                    var content = _fileSystem.File.ReadAllText(foundFile.FullName);
                    var productInfo = _serializer.Deserialize(content);

                    if (productInfo.Key == data.Key)
                    {
                        SaveToFile(foundFile.FullName, data);
                        return;
                    }
                }

                var path = _storageFolder + "/" + partialName + "_" + filesInDir.Length;
                SaveToFile(path, data);
            }, token);
        }

        private void SaveToFile(string path, Product product)
        {
            var serialized = _serializer.Serialize(product);
            _fileSystem.File.WriteAllText(path, serialized);
        }
    }
}