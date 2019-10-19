using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using IreckonuShop.API.Utilities;
using IreckonuShop.API.Utilities.LargeFileModelBinding;
using IreckonuShop.API.ViewModels;
using IreckonuShop.BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;

namespace IreckonuShop.API.Controllers
{
    [Route("api/Products")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;

        public ProductsController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        [HttpPost]
        [DisableDefaultFormModelBinding]
        public async Task<IActionResult> UploadFromCsvFile([ModelBinder(typeof(FormFileModelBinder<Product, CsvParser<Product>>))] FormFile<Product> file)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            foreach (var product in file.StreamContent())
            {
                var productModel = _mapper.Map<BusinessLogic.Models.Product>(product);
                await _productService.AddAsync(productModel, CancellationToken.None);
            }
            
            return Ok();
        }
    }
}