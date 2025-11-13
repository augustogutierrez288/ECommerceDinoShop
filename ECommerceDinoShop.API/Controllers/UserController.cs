using ECommerceDinoShop.DTO;
using ECommerceDinoShop.Service.Contract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceDinoShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("List/{role:alpha}/{search:alpha?}")]
        public async Task<IActionResult> Lista(string role, string search = "NA")
        {
            var response = new ResponseDTO<List<UserDTO>>();

            try
            {
                if (search == "NA") search = string.Empty;

                response.IsCorrect = true;
                response.Result = await _userService.List(role, search);

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
            var response = new ResponseDTO<UserDTO>();

            try
            {

                response.IsCorrect = true;
                response.Result = await _userService.Obtain(Id);

            }
            catch (Exception ex)
            {
                response.IsCorrect = false;
                response.Message = ex.Message;
            }

            return Ok(response);
        }

        [HttpPost("Create/")]
        public async Task<IActionResult> Create([FromBody] UserDTO model)
        {
            var response = new ResponseDTO<UserDTO>();

            try
            {

                response.IsCorrect = true;
                response.Result = await _userService.Create(model);

            }
            catch (Exception ex)
            {
                response.IsCorrect = false;
                response.Message = ex.Message;
            }

            return Ok(response);
        }

        [HttpPost("Authorization")]
        public async Task<IActionResult> Authorization([FromBody] LoginDTO model)
        {
            var response = new ResponseDTO<SesionDTO>();

            try
            {

                response.IsCorrect = true;
                response.Result = await _userService.Authorization(model);

            }
            catch (Exception ex)
            {
                response.IsCorrect = false;
                response.Message = ex.Message;
            }

            return Ok(response);
        }

        [HttpPut("Update")]
        public async Task<IActionResult> Update([FromBody] UserDTO model)
        {
            var response = new ResponseDTO<bool>();

            try
            {

                response.IsCorrect = true;
                response.Result = await _userService.Update(model);

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
                response.Result = await _userService.Delete(Id);

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
