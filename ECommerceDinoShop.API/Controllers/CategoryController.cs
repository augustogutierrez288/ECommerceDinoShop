using ECommerceDinoShop.DTO;
using ECommerceDinoShop.Service.Contract;
using ECommerceDinoShop.Service.Implementation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceDinoShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("List/{search:alpha?}")]
        public async Task<IActionResult> List(string search = "NA")
        {
            var response = new ResponseDTO<List<CategoryDTO>>();

            try
            {
                if (search == "NA") search = string.Empty;

                response.IsCorrect = true;
                response.Result = await _categoryService.List(search);

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
            var response = new ResponseDTO<CategoryDTO>();

            try
            {

                response.IsCorrect = true;
                response.Result = await _categoryService.Obtain(Id);

            }
            catch (Exception ex)
            {
                response.IsCorrect = false;
                response.Message = ex.Message;
            }

            return Ok(response);
        }

        [HttpPost("Create/")]
        public async Task<IActionResult> Create([FromBody] CategoryDTO model)
        {
            var response = new ResponseDTO<CategoryDTO>();

            try
            {

                response.IsCorrect = true;
                response.Result = await _categoryService.Create(model);

            }
            catch (Exception ex)
            {
                response.IsCorrect = false;
                response.Message = ex.Message;
            }

            return Ok(response);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] CategoryDTO model)
        {
            var response = new ResponseDTO<bool>();

            try
            {

                response.IsCorrect = true;
                response.Result = await _categoryService.Update(model);

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
                response.Result = await _categoryService.Delete(Id);

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
