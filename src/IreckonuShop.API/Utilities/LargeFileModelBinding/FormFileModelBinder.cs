using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace IreckonuShop.API.Utilities.LargeFileModelBinding
{
    public class FormFileModelBinder<TData, TFileParser> : IModelBinder
        where TFileParser : IFileParser<TData>, new()
    {
        private readonly TFileParser _fileParser;

        public FormFileModelBinder()
        {
            _fileParser = new TFileParser();
        }

        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
                throw new ArgumentNullException(nameof(bindingContext));

            bindingContext.Result = ModelBindingResult.Success(new FormFile<TData>(bindingContext.HttpContext.Request, _fileParser));

            return Task.CompletedTask;
        }
    }
}