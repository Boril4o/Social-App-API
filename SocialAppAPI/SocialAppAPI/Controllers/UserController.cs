using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialApp.infrastructure.Data;
using SocialAppAPI.Models.Error;
using SocialAppAPI.Models.User;
using static SocialAppAPI.Responses.ResponseMessages;

namespace SocialAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public UserController(ApplicationDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Retrieves a user from the database by their ID.
        /// </summary>
        /// <param name="id">The ID of the user to retrieve.</param>
        /// <returns>
        /// An HTTP response containing the user object as a JSON payload, or an error response if the user does not exist.
        /// </returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            // Query the database to retrieve the user with the specified ID,
            // and select only the properties that should be returned to the client.
            var user = await context
                .Users
                .Where(u => u.Id == id)
                .Select(u => new
                {
                    UserName = u.UserName,
                    ProfilePicture = u.ProfilePicture,
                    Description = u.Description ?? "",
                    Posts = u.Posts,
                    Comments = u.Comments,
                    LikedComments = u.LikedComments,
                    LikedPosts = u.LikedPosts
                })
                .FirstOrDefaultAsync();

            // If the user does not exist, return an error response with a status code of 401 (Unauthorized).
            if (user == null)
            {
                return Unauthorized(string.Format(UserIdDoesNotExist, id));
            }

            // Return an HTTP response containing the user object as a JSON payload.
            return Ok(user);
        }

        /// <summary>
        /// Retrieves a user from the database by their username.
        /// </summary>
        /// <param name="username">The username of the user to retrieve.</param>
        /// <returns>
        /// An HTTP response containing the user object as a JSON payload, or an error response.
        /// </returns>
        [HttpGet("Username/{username}")]
        public async Task<IActionResult> GetUserByUserName(string username)
        {
            // Query the database to retrieve the user with the specified username,
            // and select only the properties that should be returned to the client.
            var user = await context
                .Users
                .Where(u => u.UserName == username)
                .Select(u => new
                {
                    Id = u.Id,
                    UserName = u.UserName,
                    ProfilePicture = u.ProfilePicture,
                    Description = u.Description ?? "",
                    Posts = u.Posts,
                    Comments = u.Comments,
                    LikedComments = u.LikedComments,
                    LikedPosts = u.LikedPosts
                })
                .FirstOrDefaultAsync();

            // If the user does not exist, return an error response with a status code of 401 (Unauthorized).
            if (user == null)
            {
                return Unauthorized(string.Format(UserIdDoesNotExist, username));
            }

            // Return an HTTP response containing the user object as a JSON payload.
            return Ok(user);
        }

        /// <summary>
        /// Handles HTTP PUT requests to the endpoint with a variable user ID,
        /// updating the user's username and description properties with the provided data.
        /// </summary>
        /// <param name="id">The ID of the user to be modified.</param>
        /// <param name="modifiedUser">A ModifyUserDto object containing the modified user data sent in the body of the request.</param>
        /// <returns>
        /// An HTTP 200 status code with an empty response body if the user is successfully modified,
        /// or an HTTP 400 or 401 status code with an error message if there is an issue with the request or the user ID provided.
        /// </returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> ModifyUser(string id, [FromBody] ModifyUserDto modifiedUser)
        {
            // Retrieve the user from the database using the provided ID.
            var user = await context
               .Users
               .Where(u => u.Id == id)
               .FirstOrDefaultAsync();

            // If the user is not found, return an HTTP 401 status code with a message.
            if (user == null)
            {
                return Unauthorized(string.Format(UserIdDoesNotExist, id));
            }

            // Check if the modified username already exists in the database for any user other than the one being modified.
            var doesUserNameExist =
             await context.Users.Where(u => u.Id != id && u.UserName == modifiedUser.UserName).FirstOrDefaultAsync() == null;

            // If the username is empty or already exists, return an HTTP 400 status code with an appropriate error message.
            if (string.IsNullOrWhiteSpace(modifiedUser.UserName) || !doesUserNameExist)
            {
                var message = EmptyUserName;
                if (!doesUserNameExist)
                {
                    message = UserNameExist;
                }

                return BadRequest(message);
            }

            // Update the user's username and description properties and save changes to the database.
            user.UserName = modifiedUser.UserName;
            user.Description = modifiedUser.Description;
            await context.SaveChangesAsync();
            return Ok();
        }

        /// <summary>
        /// Handles HTTP DELETE requests to the endpoint with a variable user ID,
        /// deleting the user along with any posts, comments, likes, etc. associated with them.
        /// </summary>
        /// <param name="id">The ID of the user to be deleted.</param>
        /// <returns>
        /// An HTTP 200 status code with the deleted user object in the response body if the user and their associated data is successfully deleted,
        /// or an HTTP 401 or 500 status code with an error message if there is an issue with the request or the deletion process.
        /// </returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserById(string id)
        {
            // Retrieve the user with the provided ID from the database
            var user = await context
                .Users
                .Where(u => u.Id == id)
                .FirstOrDefaultAsync();

            // If the user does not exist, return an HTTP 401 status code with an error message
            if (user == null)
            {
                return Unauthorized(string.Format(UserIdDoesNotExist, id));
            }

            try
            {
                // Remove all comments, likes, and posts associated with the user from the database
                context.CommentsLikes.RemoveRange(user.LikedComments);
                context.Comments.RemoveRange(user.Comments);
                context.PostsLikes.RemoveRange(user.LikedPosts);
                context.Posts.RemoveRange(user.Posts);

                // Remove the user from the database
                context.Users.Remove(user);

                // Save the changes to the database
                await context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // If an exception occurs during the deletion process,
                // return an HTTP 500 status code with an error message
                return Problem(ex.Message, statusCode: 500);
            }

            // Return an HTTP 200 status code with the deleted user object in the response body
            return Ok(user);
        }
    }
}
