using ECommerceDinoShop.DTO;
using ECommerceDinoShop.Service.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceDinoShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("List/{search:alpha?}")]
        public async Task<IActionResult> List(string search = "NA")
        {
            var response = new ResponseDTO<List<ProductDTO>>();

            try
            {
                if (search == "NA") search = string.Empty;

                response.IsCorrect = true;
                response.Result = await _productService.List(search);

            }
            catch (Exception ex)
            {
                response.IsCorrect = false;
                response.Message = ex.Message;
            }

            return Ok(response);
        }

        [HttpGet("Catalog/{category}/{search?}")]
        public async Task<IActionResult> Catalog(string category, string search = "NA")
        {
            var response = new ResponseDTO<List<ProductDTO>>();

            try
            {
                if (category.ToLower() == "todos") category = string.Empty;
                if (search == "NA") search = string.Empty;

                response.IsCorrect = true;
                response.Result = await _productService.Catalog(category, search);

            }
            catch (Exception ex)
            {
                response.IsCorrect = false;
                response.Message = ex.Message;
            }

            return Ok(response);
        }

        [HttpGet("Obtain/{Id:int}")]
        public async Task<IActionResult> Obtain(int Id)
        {
            var response = new ResponseDTO<ProductDTO>();

            try
            {

                response.IsCorrect = true;
                response.Result = await _productService.Obtain(Id);

            }
            catch (Exception ex)
            {
                response.IsCorrect = false;
                response.Message = ex.Message;
            }

            return Ok(response);
        }

        [HttpPost("Create/")]
        public async Task<IActionResult> Create([FromBody] ProductDTO model)
        {
            var response = new ResponseDTO<ProductDTO>();

            try
            {

                response.IsCorrect = true;
                response.Result = await _productService.Create(model);

            }
            catch (Exception ex)
            {
                response.IsCorrect = false;
                response.Message = ex.Message;
            }

            return Ok(response);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] ProductDTO model)
        {
            var response = new ResponseDTO<bool>();

            try
            {

                response.IsCorrect = true;
                response.Result = await _productService.Update(model);

            }
            catch (Exception ex)
            {
                response.IsCorrect = false;
                response.Message = ex.Message;
            }

            return Ok(response);
        }

        [HttpDelete("Delete/{Id:int}")]
        public async Task<IActionResult> Delete(int Id)
        {
            var response = new ResponseDTO<bool>();

            try
            {

                response.IsCorrect = true;
                response.Result = await _productService.Delete(Id);

            }
            catch (Exception ex)
            {
                response.IsCorrect = false;
                response.Message = ex.Message;
            }

            return Ok(response);
        }
    }
}
