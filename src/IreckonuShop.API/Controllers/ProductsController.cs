using System.Linq;
using IreckonuShop.API.Utility;
using IreckonuShop.API.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace IreckonuShop.API.Controllers
{
    [Route("api/Products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        [HttpPost]
        [DisableDefaultFormModelBinding]
        public void UploadFromCsvFile([ModelBinder(typeof(FormFileModelBinder<Product, CsvParser<Product>>))] FormFile<Product> file)
        {
            var prod = file.StreamContent().ToList();

            var a = prod.Count;
        }
    }
}