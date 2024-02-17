using FullCartAPI.Models;
using FullCartAPI.Repositories.Implementation;
using FullCartAPI.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FullCartAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }


        [HttpGet]
        public async Task<ActionResult<List<User>>> GetAllUser()
        {
            List<User> Listall = await _userRepository.GetAllUser();
            return Ok(Listall);

        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterUser([FromBody] User userObj)
        {
            if (userObj == null)
                return BadRequest();

            // check email
            if (await _userRepository.CheckEmailExistAsync(userObj.Email))
                return BadRequest(new { Message = "Email Already Exist" });

            //check username
            if (await _userRepository.CheckUsernameExistAsync(userObj.Username))
                return BadRequest(new { Message = "Username Already Exist" });

            var passMessage = _userRepository.CheckPasswordStrength(userObj.Password);
            if (!string.IsNullOrEmpty(passMessage))
                return BadRequest(new { Message = passMessage.ToString() });

            await _userRepository.AddUser(userObj);
            return Ok(new
            {
                Status = 200,
                Message = "User Added!"
            });
        }

    }
}
