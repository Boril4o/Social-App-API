using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SocialApp.infrastructure.Data.Entities;
using SocialAppAPI.Models.Error;
using SocialAppAPI.Models.User;
using static SocialAppAPI.Responses.ResponseMessages;
using SocialAppAPI.Pictures;

namespace SocialAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;

        public UserController(SignInManager<User> signInManager, UserManager<User> userManager)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(UserRegisterDto userDto)
        {
            // Check if the model state is valid
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Create a new User object with the properties from the UserRegisterDto object
            var user = new User
            {
                UserName = userDto.Username,
                ProfilePicture = PictureConverter.GetBytesFromPicture(),
            };

            try
            {
                // Attempt to create a new user using the UserManager service
                var result = await userManager.CreateAsync(user, userDto.Password);

                // If user creation fails, return a Bad Request response
                if (!result.Succeeded)
                {
                    return BadRequest(UserRegistrationFailed);
                }

                // If user creation succeeds, return an OK response
                return Ok(UserRegistrationSuccessful);
            }
            catch (Exception ex)
            {
                // If an exception occurs, log the error and return a Problem response
                var message = new ErrorModel(Error, ex.Message).ToString();
                return Problem(message,
                    statusCode: 500);
            }
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginUserDto userDto)
        {
            // Find the user with the specified username
            var user = await userManager.FindByNameAsync(userDto.Username);

            // If the user is null, return a Bad Request response indicating that the user does not exist
            if (user == null)
            {
                return BadRequest(UserDoesNotExist);
            }

            try
            {
                // Attempt to sign in the user with the specified username and password
                var result = await signInManager.PasswordSignInAsync(user, userDto.Password, false, false);

                // If the sign-in attempt fails, return an Unauthorized response
                if (!result.Succeeded)
                {
                    return Unauthorized(userDto);
                }

                // If the sign-in attempt succeeds, return an OK response
                return Ok();
            }
            catch (Exception ex)
            {
                // If an exception occurs, log the error and return a Problem response
                var message = new ErrorModel(Error, ex.Message).ToString();
                return Problem(message,
                    statusCode: 500);
            }
        }
    }
}
