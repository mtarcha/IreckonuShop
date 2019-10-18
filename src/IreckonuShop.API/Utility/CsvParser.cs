using System.Collections.Generic;
using System.IO;
using CsvHelper;

namespace IreckonuShop.API.Utility
{
    public class CsvParser<TData> : IFileParser<TData>
    {
        public IEnumerable<TData> Parse(TextReader textReader)
        {
            using (var csv = new CsvReader(textReader))
            {
                foreach (var data in csv.GetRecords<TData>())
                {
                    yield return data;
                }
            }
        }
    }
}