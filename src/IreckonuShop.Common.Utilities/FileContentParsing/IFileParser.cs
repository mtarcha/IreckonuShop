using System.Collections.Generic;
using System.IO;

namespace IreckonuShop.Common.Utilities.FileContentParsing
{
    public interface IFileParser<out TData>
    {
        IEnumerable<TData> Parse(Stream stream);
    }
}