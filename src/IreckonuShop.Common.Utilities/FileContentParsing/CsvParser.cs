using System.Collections.Generic;
using System.IO;
using CsvHelper;

namespace IreckonuShop.Common.Utilities.FileContentParsing
{
    public class CsvParser<TData> : IFileParser<TData>
    {
        public IEnumerable<TData> Parse(Stream stream)
        {
            using (var streamReader = new StreamReader(stream))
            {
                using (var csv = new CsvReader(streamReader))
                {
                    foreach (var data in csv.GetRecords<TData>())
                    {
                        yield return data;
                    }
                }
            }
        }
    }
}