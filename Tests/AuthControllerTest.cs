using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

using api_pim.Interfaces;
using api_pim.Controllers;
using api_pim.Models;

namespace Tests
{
    [TestFixture]
    public class AuthControllerTest
    {
        private Mock<IAuthService> authServiceMock = null!;
        private AuthController authController = null!;

        [SetUp]
        public void Setup()
        {
            authServiceMock = new Mock<IAuthService>();
            authController = new AuthController(authServiceMock.Object);
        }

        [Test]
        public void Login_WithValidCredentials_ReturnsOkResult()
        {
            var expected_token = "fake_token";


            var loginRequest = new LoginRequest
            {
                Email = "teste@gmail",
                Senha = "12qwaszx"
            };


            authServiceMock.Setup(service => service.Authenticate("teste@gmail", "12qwaszx")).Returns(expected_token);


            var result = authController.Login(loginRequest) as OkObjectResult;
            var resultValue = result!.Value as TokenResponse;


            Assert.NotNull(result);
            Assert.AreEqual((int)HttpStatusCode.OK, result.StatusCode);
            Assert.AreEqual(expected_token, resultValue!.Token);
        }

        [Test]
        public void Login_WithInvalidCredentials_ReturnsUnauthorizedResult()
        {

            var loginRequest = new LoginRequest
            {
                Email = "teste@gmail",
                Senha = "12qwaszx"
            };

            authServiceMock.Setup(service => service.Authenticate("teste@gmail", "12qwaszx")).Returns(string.Empty);

            var result = authController.Login(loginRequest) as UnauthorizedObjectResult;
            var resultValue = result!.Value as TokenResponse;


            Assert.NotNull(result);
            Assert.AreEqual((int)HttpStatusCode.Unauthorized, result.StatusCode);
        }
    }

}
