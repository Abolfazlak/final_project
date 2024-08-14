using Moq;
using RiskManagement.API.RiskManagement.Models.Users;
using RiskManagement.API.RiskManagement.Services;
using RiskManagement.API.RiskManagement.Helpers.Interfaces;
using RiskManagement.API.RiskManagement.Repositories.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Http;
using RiskManagement.API.RiskManagement.DataProvide;

namespace RiskManagement.Test.User
{
    [TestFixture]
    public class UserServiceTests
    {
        private Mock<ICommonHelper> _mockHelper;
        private Mock<IUserRepo> _mockUserRepo;
        private UserService _userService;

        [SetUp]
        public void SetUp()
        {
            _mockHelper = new Mock<ICommonHelper>();
            _mockUserRepo = new Mock<IUserRepo>();
            _userService = new UserService(_mockHelper.Object, _mockUserRepo.Object);
        }

        [Test]
        public async Task RegisterAdmin_UserAlreadyExists_ReturnsBadRequest()
        {
            // Arrange
            var dto = new AddAdminUserDto { UserName = "testUser", CompanyName = "testCompany" };
            _mockUserRepo.Setup(x => x.CheckUserExists(dto.UserName)).ReturnsAsync(true);

            // Act
            var result = await _userService.RegisterAdmin(dto);

            Assert.Multiple(() =>
            {
                // Assert
                Assert.That(result.Code, Is.EqualTo(400));
                Assert.That(result.Content, Is.EqualTo("نام کاربری تکراری است."));
            });
        }

        [Test]
        public async Task RegisterAdmin_CompanyDoesNotExist_CreatesCompanyAndUser()
        {
            // Arrange
            var dto = new AddAdminUserDto { UserName = "testUser", CompanyName = "testCompany", Password = "password" };
            _mockUserRepo.Setup(x => x.CheckUserExists(dto.UserName)).ReturnsAsync(false);
            _mockUserRepo.Setup(x => x.GetCompanyByName(dto.CompanyName)).ReturnsAsync((Company)null);
            _mockUserRepo.Setup(x => x.AddCompanyToDb(It.IsAny<Company>())).Returns(Task.CompletedTask);
            _mockUserRepo.Setup(x => x.GetCompanyByName(dto.CompanyName)).ReturnsAsync(new Company { Id = 1, CompanyName = "testCompany" });
            _mockUserRepo.Setup(x => x.AddUserToDb(It.IsAny<API.RiskManagement.DataProvide.User>())).Returns(Task.CompletedTask);
            _mockHelper.Setup(x => x.GetHash(dto.Password)).Returns("hashedPassword");

            // Act
            var result = await _userService.RegisterAdmin(dto);

            Assert.Multiple(() =>
            {
                // Assert
                Assert.That(result.Code, Is.EqualTo(200));
                Assert.That(result.Content, Is.EqualTo("عملیات با موفقیت انجام شد."));
            });
        }
        
                
        [Test]
        public async Task RegisterAdmin_CompanyCreationFails_ReturnsInternalServerError()
        {
            // Arrange
            var dto = new AddAdminUserDto { UserName = "testUser", CompanyName = "testCompany", Password = "password" };
            _mockUserRepo.Setup(x => x.CheckUserExists(dto.UserName)).ReturnsAsync(false);
            _mockUserRepo.Setup(x => x.GetCompanyByName(dto.CompanyName)).ReturnsAsync((Company)null);
            _mockUserRepo.Setup(x => x.AddCompanyToDb(It.IsAny<Company>())).Returns(Task.CompletedTask);
            _mockUserRepo.Setup(x => x.GetCompanyByName(dto.CompanyName)).ReturnsAsync((Company)null); // Simulate failure

            // Act
            var result = await _userService.RegisterAdmin(dto);

            Assert.Multiple(() =>
            {
                // Assert
                Assert.That(result.Code, Is.EqualTo(500));
                Assert.That(result.Content, Is.EqualTo("عملیات با خطا مواجه شد!"));
            });
        }
        
        [Test]
        public async Task RegisterAdmin_ExceptionDuringUserCreation_ReturnsInternalServerError()
        {
            // Arrange
            var dto = new AddAdminUserDto { UserName = "testUser", CompanyName = "testCompany", Password = "password" };
            var company = new Company { Id = 1, CompanyName = "testCompany" };
            _mockUserRepo.Setup(x => x.CheckUserExists(dto.UserName)).ReturnsAsync(false);
            _mockUserRepo.Setup(x => x.GetCompanyByName(dto.CompanyName)).ReturnsAsync(company);
            _mockUserRepo.Setup(x => x.AddUserToDb(It.IsAny<API.RiskManagement.DataProvide.User>())).ThrowsAsync(new Exception("User creation error"));
            _mockHelper.Setup(x => x.GetHash(dto.Password)).Returns("hashedPassword");

            // Act
            var result = await _userService.RegisterAdmin(dto);

            // Assert
            Assert.That(result.Code, Is.EqualTo(500));
            Assert.AreEqual("عملیات با خطا مواجه شد!User creation error", result.Content);
        }


        [Test]
        public async Task RegisterUser_UserAlreadyExists_ReturnsBadRequest()
        {
            // Arrange
            var dto = new AddUserDto { UserName = "testUser" };
            _mockUserRepo.Setup(x => x.CheckUserExists(dto.UserName)).ReturnsAsync(true);

            // Act
            var result = await _userService.RegisterUser(dto, 1);

            Assert.Multiple(() =>
            {
                // Assert
                Assert.That(result.Code, Is.EqualTo(400));
                Assert.That(result.Content, Is.EqualTo("نام کاربری تکراری است."));
            });
        }
        
        [Test]
        public async Task RegisterUser_DatabaseError_ReturnsInternalServerError()
        {
            // Arrange
            var dto = new AddUserDto { UserName = "testUser", Password = "password" };
            _mockUserRepo.Setup(x => x.CheckUserExists(dto.UserName)).ReturnsAsync(false);
            _mockUserRepo.Setup(x => x.AddUserToDb(It.IsAny<API.RiskManagement.DataProvide.User>())).ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _userService.RegisterUser(dto, 1);

            // Assert
            Assert.AreEqual(500, result.Code);
            Assert.AreEqual("عملیات با خطا مواجه شد!Database error", result.Content);
        }

        [Test]
        public async Task Login_InvalidUsername_ReturnsUnauthorized()
        {
            // Arrange
            var loginModel = new LoginModel { Username = "testUser", Password = "password" };
            _mockUserRepo.Setup(x => x.GetUserByUserName(loginModel.Username)).ReturnsAsync((API.RiskManagement.DataProvide.User)null);

            // Act
            var result = await _userService.Login(loginModel);

            Assert.Multiple(() =>
            {
                // Assert
                Assert.That(result.Code, Is.EqualTo(401));
            });
        }

        [Test]
        public async Task Login_InvalidPassword_ReturnsUnauthorized()
        {
            // Arrange
            var user = new API.RiskManagement.DataProvide.User { UserName = "testUser", Password = "hashedPassword" };
            var loginModel = new LoginModel { Username = "testUser", Password = "wrongPassword" };

            _mockUserRepo.Setup(x => x.GetUserByUserName(loginModel.Username)).ReturnsAsync(user);
            _mockHelper.Setup(x => x.GetHash(loginModel.Password)).Returns("wrongHashedPassword");

            // Act
            var result = await _userService.Login(loginModel);

            Assert.Multiple(() =>
            {
                // Assert
                Assert.That(result.Code, Is.EqualTo(401));
            });
        }

        [Test]
        public async Task Login_ValidCredentials_ReturnsToken()
        {
            // Arrange
            var user = new API.RiskManagement.DataProvide.User { Id = 1, UserName = "testUser", Password = "hashedPassword", IsAdmin = true, CompanyId = 1 };
            var loginModel = new LoginModel { Username = "testUser", Password = "password" };

            _mockUserRepo.Setup(x => x.GetUserByUserName(loginModel.Username)).ReturnsAsync(user);
            _mockHelper.Setup(x => x.GetHash(loginModel.Password)).Returns("hashedPassword");
            _mockHelper.Setup(x => x.GenerateAccessToken(user.Id, user.IsAdmin, user.CompanyId)).Returns(new JwtSecurityToken());

            // Act
            var result = await _userService.Login(loginModel);

            Assert.Multiple(() =>
            {
                // Assert
                Assert.That(result.Code, Is.EqualTo(200));
                Assert.That(result.Content, Is.Not.Null);
            });
        }
        
        [Test]
        public async Task Login_UnexpectedException_ReturnsInternalServerError()
        {
            // Arrange
            var loginModel = new LoginModel { Username = "testUser", Password = "password" };
            _mockUserRepo.Setup(x => x.GetUserByUserName(loginModel.Username)).ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _userService.Login(loginModel);

            // Assert
            Assert.AreEqual(500, result.Code);
        }

        
        [Test]
        public async Task Login_ValidPasswordButNoTokenGenerated_ReturnsInternalServerError()
        {
            // Arrange
            var user = new API.RiskManagement.DataProvide.User { Id = 1, UserName = "testUser", Password = "hashedPassword", IsAdmin = true, CompanyId = 1 };
            var loginModel = new LoginModel { Username = "testUser", Password = "password" };

            _mockUserRepo.Setup(x => x.GetUserByUserName(loginModel.Username)).ReturnsAsync(user);
            _mockHelper.Setup(x => x.GetHash(loginModel.Password)).Returns("hashedPassword");
            _mockHelper.Setup(x => x.GenerateAccessToken(user.Id, user.IsAdmin, user.CompanyId)).Returns<JwtSecurityToken>(null);

            // Act
            var result = await _userService.Login(loginModel);

            // Assert
            Assert.AreEqual(500, result.Code);
        }

        

        [Test]
        public async Task GetAllUsersService_ValidRequest_ReturnsUsers()
        {
            // Arrange
            var httpContext = new DefaultHttpContext
            {
                Items =
                {
                    ["companyId"] = "1"
                }
            };
            var users = new List<UserDto> { new UserDto { Id = 1, UserName = "user1" }, new UserDto { Id = 2, UserName = "user2" } };

            _mockUserRepo.Setup(x => x.GetUsersByCompany(1)).ReturnsAsync(users);

            // Act
            var result = await _userService.GetAllUsersService(httpContext);

            // Assert
            Assert.AreEqual(200, result.Code);
            Assert.AreEqual(users, result.Content);
        }
        
        [Test]
        public async Task GetAllUsersService_NoUsersFound_ReturnsEmptyList()
        {
            // Arrange
            var httpContext = new DefaultHttpContext
            {
                Items =
                {
                    ["companyId"] = "1"
                }
            };
            _mockUserRepo.Setup(x => x.GetUsersByCompany(1)).ReturnsAsync(new List<UserDto>());

            // Act
            var result = await _userService.GetAllUsersService(httpContext);

            Assert.Multiple(() =>
            {
                // Assert
                Assert.That(result.Code, Is.EqualTo(200));
                Assert.That(result.Content, Is.Not.Null);
            });
            Assert.That(result.Content, Is.Empty);
        }


        [Test]
        public async Task GetUserByIdService_UserDoesNotExist_ReturnsNotFound()
        {
            // Arrange
            _mockUserRepo.Setup(x => x.GetUserByّId(1)).ReturnsAsync((API.RiskManagement.DataProvide.User)null);

            // Act
            var result = await _userService.GetUserByIdService(1);

            Assert.Multiple(() =>
            {
                // Assert
                Assert.That(result.Code, Is.EqualTo(404));
                Assert.That(result.Content, Is.Null);
            });
        }
        
        [Test]
        public async Task GetUserByIdService_NullUserId_ReturnsNotFound()
        {
            // Act
            var result = await _userService.GetUserByIdService(null);

            // Assert
            Assert.AreEqual(404, result.Code);
            Assert.IsNull(result.Content);
        }
        
        [Test]
        public async Task GetUserByIdService_ExceptionThrown_ReturnsInternalServerError()
        {
            // Arrange
            _mockUserRepo.Setup(x => x.GetUserByّId(It.IsAny<long>())).ThrowsAsync(new Exception("Unexpected error"));

            // Act
            var result = await _userService.GetUserByIdService(1);

            // Assert
            Assert.AreEqual(500, result.Code);
            Assert.IsNull(result.Content);
        }



    }
}

