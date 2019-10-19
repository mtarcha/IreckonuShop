﻿using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.WebUtilities;

namespace IreckonuShop.API.Utilities.LargeFileModelBinding
{  
    public class FormFile<TData>
    {
        private readonly HttpRequest _request;
        private readonly IFileParser<TData> _fileParser;

        public FormFile(HttpRequest request, IFileParser<TData> fileParser)
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
                using (var streamReader = new StreamReader(section.Body))
                {
                    foreach (var data in _fileParser.Parse(streamReader))
                    {
                        yield return data;
                    }
                }

                section = reader.ReadNextSectionAsync().Result;
            }
        }
    }
}