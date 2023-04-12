using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using SocialApp.infrastructure.Data;
using SocialAppAPI.Controllers;
using static SocialAppAPI.Responses.ResponseMessages;
using SocialApp.infrastructure.Data.Entities;
using SocialAppAPI.Models.User;

namespace SocialMediaApp.UnitTests
{
    [TestFixture]
    public class UserControllerTests
    {
        private UserController userController = null!;
        private ApplicationDbContext context = null!;

        [SetUp]
        public void SetUp()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "Db")
                .Options;

            context = new ApplicationDbContext(options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            userController = new UserController(context);
        }

        [Test]
        public async Task When_Executing_The_GetUserById_Method_If_User_Does_Not_Exist_Should_Return_Unauthorized()
        {
            //Arrange
            const string id = "something";
            var expectedMessage = string.Format(UserIdDoesNotExist, id);

            //Act
            var result = await userController.GetUserById(id);

            //Assert
            Assert.IsInstanceOf<UnauthorizedObjectResult>(result,
                $"Response should be of type {nameof(UnauthorizedObjectResult)}" +
                $" but was {result.GetType().Name}");

            var message = ((UnauthorizedObjectResult)result).Value?.ToString();
            Assert.That(expectedMessage == message,
                $"Response message should be {expectedMessage} but was {message}");
        }

        [Test]
        public async Task When_GetUserById_Method_Is_Successful_Should_Return_Ok_Response_With_User()
        {
            //Arrange
            const string firstUserName = "Test";
            const string firstId = "id";
            const string secondUserName = "Test1";
            const string secondId = "id123";
            
            var User = new User()
            {
                UserName = firstUserName,
                Id = firstId,
                ProfilePicture = new byte[2]
            };

            var User1 = new User()
            {
                UserName = secondUserName,
                Id = secondId,
                ProfilePicture = new byte[2]
            };

            await context.Users.AddRangeAsync(User, User1);
            await context.SaveChangesAsync();

            //Act
            var firstResult = await userController.GetUserById(firstId);
            var secondResult = await userController.GetUserById(secondId);
            var firstResultDto = ((OkObjectResult)firstResult).Value as UserDto;
            var secondResultDto = ((OkObjectResult)secondResult).Value as UserDto;

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(firstResult);
            Assert.IsInstanceOf<OkObjectResult>(secondResult);
            Assert.IsInstanceOf<UserDto>(((OkObjectResult)firstResult).Value);
            Assert.IsInstanceOf<UserDto>(((OkObjectResult)secondResult).Value);
            Assert.That(firstResultDto?.UserName == firstUserName && firstResultDto.Id == firstId);
            Assert.That(secondResultDto?.UserName == secondUserName && secondResultDto.Id == secondId);
        }

        [Test]
        public async Task When_Executing_The_GetUserByUserName_Method_If_User_Does_Not_Exist_Should_Return_Unauthorized()
        {
            //Arrange
            const string username = "username1";
            var expectedMessage = string.Format(UserNameDoesNotExist, username);

            //Act
            var result = await userController.GetUserByUserName(username);

            //Assert
            Assert.IsInstanceOf<UnauthorizedObjectResult>(result,
                $"Response should be of type {nameof(UnauthorizedObjectResult)}" +
                $" but was {result.GetType().Name}");

            var message = ((UnauthorizedObjectResult)result).Value?.ToString();
            Assert.That(expectedMessage == message,
                $"Response message should be {expectedMessage} but was {message}");
        }

        [Test]
        public async Task When_GetUserByUserName_Method_Is_Successful_Should_Return_Ok_Response_With_User()
        {
            //Arrange
            const string firstUserName = "Test";
            const string firstId = "id";
            const string secondUserName = "Test1";
            const string secondId = "id123";

            var User = new User()
            {
                UserName = firstUserName,
                Id = firstId,
                ProfilePicture = new byte[2]
            };

            var User1 = new User()
            {
                UserName = secondUserName,
                Id = secondId,
                ProfilePicture = new byte[2]
            };

            await context.Users.AddRangeAsync(User, User1);
            await context.SaveChangesAsync();

            //Act
            var firstResult = await userController.GetUserByUserName(firstUserName);
            var secondResult = await userController.GetUserByUserName(secondUserName);
            var firstResultDto = ((OkObjectResult)firstResult).Value as UserDto;
            var secondResultDto = ((OkObjectResult)secondResult).Value as UserDto;

            //Assert
            Assert.IsInstanceOf<OkObjectResult>(firstResult);
            Assert.IsInstanceOf<OkObjectResult>(secondResult);
            Assert.IsInstanceOf<UserDto>(((OkObjectResult)firstResult).Value);
            Assert.IsInstanceOf<UserDto>(((OkObjectResult)secondResult).Value);
            Assert.That(firstResultDto?.UserName == firstUserName && firstResultDto.Id == firstId);
            Assert.That(secondResultDto?.UserName == secondUserName && secondResultDto.Id == secondId);
        }


    }
}
