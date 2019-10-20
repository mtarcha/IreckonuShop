using System;
using System.Threading.Tasks;
using IreckonuShop.Common.Utilities.FileContentParsing;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace IreckonuShop.Common.Utilities.CustomModelBinding.LargeFormFile
{
    public class LargeFormFileModelBinder<TData, TFileParser> : IModelBinder
        where TFileParser : IFileParser<TData>, new()
    {
        private readonly TFileParser _fileParser;

        public LargeFormFileModelBinder()
        {
            _fileParser = new TFileParser();
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
                throw new ArgumentNullException(nameof(bindingContext));

            bindingContext.Result = ModelBindingResult.Success(new LargeFormFile<TData>(bindingContext.HttpContext.Request, _fileParser));

            return Task.CompletedTask;
        }
    }
}