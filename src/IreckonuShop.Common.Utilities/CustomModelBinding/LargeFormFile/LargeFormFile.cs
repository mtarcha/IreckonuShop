using System.Collections.Generic;
using System.IO;
using IreckonuShop.Common.Utilities.FileContentParsing;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.WebUtilities;

namespace IreckonuShop.Common.Utilities.CustomModelBinding.LargeFormFile
{  
    public class LargeFormFile<TData>
    {
        private readonly HttpRequest _request;
        private readonly IFileParser<TData> _fileParser;

        public LargeFormFile(HttpRequest request, IFileParser<TData> fileParser)
        {
            _request = request;
            _fileParser = fileParser;
        }

        public IEnumerable<TData> StreamContent()
        {
            var boundary = _request.GetMultipartBoundary();
            var reader = new MultipartReader(boundary, _request.Body);
            var section = reader.ReadNextSectionAsync().Result;

            while (section != null)
            {
                foreach (var data in _fileParser.Parse(section.Body))
                {
                    yield return data;
                }

                section = reader.ReadNextSectionAsync().Result;
            }
        }
    }
}