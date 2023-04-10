using Castle.Components.DictionaryAdapter.Xml;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using SocialApp.infrastructure.Data.Entities;
using SocialAppAPI.Controllers;
using SocialAppAPI.Models.User;

namespace SocialMediaApp.UnitTests
{
    [TestFixture]
    public class AccountControllerTests
    {
        private Mock<UserManager<User>> userManager = null!;
        private Mock<SignInManager<User>> signInManager = null!;
        private AccountController userController = null!;

        [SetUp]
        public void Setup()
        {
            userManager = new Mock<UserManager<User>>(Mock.Of<IUserStore<User>>(),
             null, null, null, null, null, null, null, null);

            //var a = new SignInManager<User>(userManager.Object, new Mock<IHttpContextAccessor>().Object, new Mock<IUserClaimsPrincipalFactory<User>>().Object, new Mock<IOptions<IdentityOptions>>().Object, new Mock<ILogger<SignInManager<User>>>().Object, new Mock<IAuthenticationSchemeProvider>().Object, new Mock<IUserConfirmation<User>>().Object);

            signInManager = new Mock<SignInManager<User>>(userManager.Object, new Mock<IHttpContextAccessor>().Object, new Mock<IUserClaimsPrincipalFactory<User>>().Object, new Mock<IOptions<IdentityOptions>>().Object, new Mock<ILogger<SignInManager<User>>>().Object, new Mock<IAuthenticationSchemeProvider>().Object, new Mock<IUserConfirmation<User>>().Object);

            userController = new (signInManager.Object, userManager.Object);
        }

        [Test]
        public async Task When_RegisterModel_Is_Invalid_Should_Return_BadRequest()
        {
            // Arrange
            var userDto = new UserRegisterDto
            {
                Username = "",
                Password = ""
            };

            userController.ModelState.AddModelError("Username", "The Username field is required.");

            //Act
            var result = await userController.Register(userDto);

            //Assert
            Assert.IsInstanceOf<BadRequestObjectResult>(result, "API did not return BadRequest");
        }
    }
}