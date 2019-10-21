using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities.Resources;
using NUnit.Framework;

namespace IreckonuShop.PerformanceTests
{
    [TestFixture]
    public class UploadProductsFileTests
    {
        private IMemoryObserver _memoryObserver;

        [SetUp]
        public void Setup()
        {
            _memoryObserver = new MemoryObserver();
        }

        [Test]
        public async Task Test()
        {
            var appFactory = new WebApplicationFactory<API.Startup>().WithWebHostBuilder(x => x.UseEnvironment("Sql"));
           
            var client = appFactory.CreateClient();

            var memoryUsage = new double[3];

            long fileSize = 50 * (1 << 10);
            for (var i = 0; i < 3; i++)
            {
                Action<HttpContent> upload = content => client.PostAsync("api/products/upload-csv", content).Wait();
                
                memoryUsage[i] = Conversion.BytesAsMebibytes(UploadFile(upload, fileSize));

                fileSize *= 5;
            }

            var a = memoryUsage;
        }

        private long UploadFile(Action<HttpContent> upload, long fileSizeInBytes)
        {
            using (var content = new MultipartFormDataContent("Upload----" + DateTime.Now.ToString(CultureInfo.InvariantCulture)))
            {
                var csv = GenerateLargeCsv(fileSizeInBytes);
                content.Add(new StringContent(csv), "file", "inMem.csv");

                var process = Process.GetCurrentProcess();
                _memoryObserver.LogPrivateBytes(0);
                _memoryObserver.LogInitialUsedBytes(process.PrivateMemorySize64);
                StartPerformanceMonitoring(process);

                upload(content);
            }

            return _memoryObserver.MaximumMemoryUsageIncrease;
        }

        public interface IMemoryObserver
        {
            long PrivateBytesMaximumObserved { get; }
            long MaximumMemoryUsageIncrease { get; }

            void LogPrivateBytes(long privateBytes);

            void LogInitialUsedBytes(long privateBytes);
        }

        internal sealed class MemoryObserver : IMemoryObserver
        {
            private long _privateBytesInitial;

            public MemoryObserver()
            {
                PrivateBytesMaximumObserved = -1;
            }

            public long PrivateBytesMaximumObserved { get; private set; }

            public long MaximumMemoryUsageIncrease => PrivateBytesMaximumObserved - _privateBytesInitial;

            public void LogPrivateBytes(long privateBytes)
            {
                if (privateBytes <= PrivateBytesMaximumObserved)
                {
                    return;
                }

                PrivateBytesMaximumObserved = privateBytes;
            }

            public void LogInitialUsedBytes(long privateBytes)
            {
                _privateBytesInitial = privateBytes;
            }
        }

        private void StartPerformanceMonitoring(Process process)
        {
            Task.Factory.StartNew(
                async () =>
                {
                    while (!process.HasExited)
                    {
                        process.Refresh();
                        var privateMemoryBytes = process.PrivateMemorySize64;

                        _memoryObserver.LogPrivateBytes(privateMemoryBytes);

                        await Task.Delay(1000);
                    }
                },
                TaskCreationOptions.LongRunning);
        }

        private string GenerateLargeCsv(long bytes)
        {
            var csvContent = new StringBuilder();
            csvContent.AppendLine("Key,ArtikelCode,ColorCode,Description,Price,DiscountPrice,DeliveredIn,Q1,Size,Color");

            Func<string> generateLine = () => $"{Guid.NewGuid()},2,broek,Gaastra,8,0,1-3 werkdagen,baby,104,grijs";

            var sizeOfOneLine = ASCIIEncoding.Unicode.GetByteCount(generateLine());
            var requiredLines = Math.Ceiling(bytes / (double)sizeOfOneLine);

            for (int i = 0; i < requiredLines; i++)
            {
                csvContent.AppendLine(generateLine());
            }

            return csvContent.ToString();
        }

        public static class Conversion
        {
            private const long BytesInMebibyte = 1 << 20;

            public static double BytesAsMebibytes(long bytes)
            {
                return (double)bytes / BytesInMebibyte;
            }

            public static long MebibytesAsBytes(double mebibytes)
            {
                checked
                {
                    return (long)((mebibytes * BytesInMebibyte) + (mebibytes >= 0 ? 0.5 : -0.5));
                }
            }
        }
    }
}