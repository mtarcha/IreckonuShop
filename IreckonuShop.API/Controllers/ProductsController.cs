using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using IreckonuShop.API.Utility;
using IreckonuShop.API.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace IreckonuShop.API.Controllers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class DisableDefaultFormModelBindingAttribute : ModelBinderAttribute, IResourceFilter
    {
        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            var factories = context.ValueProviderFactories;
            factories.RemoveType<FormValueProviderFactory>();
            factories.RemoveType<JQueryFormValueProviderFactory>();
        }

        public void OnResourceExecuted(ResourceExecutedContext context)
        {
        }
    }

    [Route("api/Products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        [HttpPost]
        [DisableDefaultFormModelBinding]
        public void UploadFromCsvFile([ModelBinder(typeof(FormFileModelBinder<Product, CsvParser<Product>>))] FormFile<Product> file)
        {
            var prod = file.StreamContent(CancellationToken.None).ToList();

            var a = prod.Count;
        }
    }

    public class FormFileModelBinder<TData, TFileParser> : IModelBinder where TFileParser : IFileParser<TData>, new()
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