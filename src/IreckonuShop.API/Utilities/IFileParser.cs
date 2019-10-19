using System.Collections.Generic;
using System.IO;

namespace IreckonuShop.API.Utilities
{
    public interface IFileParser<out TData>
    {
        IEnumerable<TData> Parse(TextReader textReader);
    }
}