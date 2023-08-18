namespace Playstudios.Testing.Functions
{
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json.Linq;
    using Playstudios.Api.Functions;
    using Playstudios.Common.Enumerations;
    using Playstudios.Common.Models.Dto;
    using Playstudios.Testing.Functions.Base;
    using Xunit;

    public class UserFunctionTest : FunctionsTestBase
    {
        private UserFunction userFunction;

        public UserFunctionTest()
        {
            databaseContextMock
                .Setup(context => context.Users)
                .Returns(TestDataFactory.GetUserMock().Object);

            sendinblueMock
                .Setup(email => email.SendResetPasswordCode("", "", ""))
                .Returns(true);

            userFunction = new UserFunction(userManager);
        }

        [Theory]
        [InlineData(false, "enrique@hotmail.com", "1234567","Gutierez", "Luis", ErrorCodesEnum.InvalidUserRequest)]
        [InlineData(true, null, "1234567", "Gutierez", "Luis", ErrorCodesEnum.InvalidUserRequestEmail)]
        [InlineData(true, "", "1234567", "Gutierez", "Luis", ErrorCodesEnum.InvalidUserRequestEmail)]
        [InlineData(true, " ", "1234567", "Gutierez", "Luis", ErrorCodesEnum.InvalidUserRequestEmail)]
        [InlineData(true, "enriquehotmail.com", "1234567", "Gutierez", "Luis", ErrorCodesEnum.InvalidUserRequestEmailFormat)]
        [InlineData(true, "enrique@hotmail.com", null, "Gutierez", "Luis", ErrorCodesEnum.InvalidUserRequestPassword)]
        [InlineData(true, "enrique@hotmail.com", "", "Gutierez", "Luis", ErrorCodesEnum.InvalidUserRequestPassword)]
        [InlineData(true, "enrique@hotmail.com", " ", "Gutierez", "Luis", ErrorCodesEnum.InvalidUserRequestPassword)]
        [InlineData(true, "enrique@hotmail.com", "1234567", null, "Luis", ErrorCodesEnum.InvalidUserRequestLastName)]
        [InlineData(true, "enrique@hotmail.com", "1234567", "", "Luis", ErrorCodesEnum.InvalidUserRequestLastName)]
        [InlineData(true, "enrique@hotmail.com", "1234567", "   ", "Luis", ErrorCodesEnum.InvalidUserRequestLastName)]
        [InlineData(true, "enrique@hotmail.com", "1234567", "Gutierez", null, ErrorCodesEnum.InvalidUserRequestName)]
        [InlineData(true, "enrique@hotmail.com", "1234567", "Gutierez", "", ErrorCodesEnum.InvalidUserRequestName)]
        [InlineData(true, "enrique@hotmail.com", "1234567", "Gutierez", "   ", ErrorCodesEnum.InvalidUserRequestName)]
        [InlineData(true, "alanfma@hotmail.com", "1234567", "Gutierez", "Luis", ErrorCodesEnum.UserEmailAllreadyUsed)]
        public async void CreateUser_ShouldBeBadRequestAsync(bool hasDto, string email, string password, string lastName, string name, ErrorCodesEnum error)
        {
            var request = TestFactory.CreateHttpRequest(body: hasDto ? JObject.FromObject(CreateCreateUserDto(new List<Action<CreateUserDto>>
                {
                    obj => obj.Email = email,
                    obj => obj.Password = password,
                    obj => obj.LastName = lastName,
                    obj => obj.Name = name,
                })) : null);

            var result = await userFunction
                .CreateUser(request)
                .ConfigureAwait(false);

            Assert.True(result.GetType() == typeof(BadRequestObjectResult), "Assert result is BadRequestObjectResult type.");

            var response = (ErrorDto)((BadRequestObjectResult)result).Value;

            Assertions.ErrorDefinitionIsNotNullOrEmpty(response);

            Assert.True(
                error.ToString().Equals(response.Code, StringComparison.InvariantCultureIgnoreCase),
                $"Assert ErrorDefinition.Code is {error}.");
        }

        [Fact]
        public async void CreateUser_ShouldBeSuccessful()
        {
            var request = TestFactory.CreateHttpRequest(body: JObject.FromObject(CreateCreateUserDto()));

            var result = await userFunction
                .CreateUser(request)
                .ConfigureAwait(false);

            Assert.NotNull(result);
            Assert.True(result.GetType() == typeof(OkObjectResult), "Assert is OkObjectResult type.");

            var definition = (UserDto)((OkObjectResult)result).Value;

            Assert.NotNull(definition);
        }

        [Theory]
        [InlineData(false, "alanfma@hotmail.com", ErrorCodesEnum.InvalidResetPasswordRequest)]
        [InlineData(true, null,  ErrorCodesEnum.InvalidResetPasswordRequestEmail)]
        [InlineData(true, "", ErrorCodesEnum.InvalidResetPasswordRequestEmail)]
        [InlineData(true, " ",  ErrorCodesEnum.InvalidResetPasswordRequestEmail)]
        [InlineData(true, "alanfmahotmail.com", ErrorCodesEnum.InvalidResetPasswordRequestEmailFormat)]
        [InlineData(true, "enrique@hotmail.com", ErrorCodesEnum.UserEmailNotFound)]
        public async void ResetPassword_ShouldBeBadRequestAsync(bool hasDto, string email, ErrorCodesEnum error)
        {
            var request = TestFactory.CreateHttpRequest(body: hasDto ? JObject.FromObject(CreateResetPasswordDto(new List<Action<ResetPasswordDto>>
                {
                    obj => obj.Email = email
                })) : null);

            var result = await userFunction
                .ResetPassword(request)
                .ConfigureAwait(false);

            Assert.True(result.GetType() == typeof(BadRequestObjectResult), "Assert result is BadRequestObjectResult type.");

            var response = (ErrorDto)((BadRequestObjectResult)result).Value;

            Assertions.ErrorDefinitionIsNotNullOrEmpty(response);

            Assert.True(
                error.ToString().Equals(response.Code, StringComparison.InvariantCultureIgnoreCase),
                $"Assert ErrorDefinition.Code is {error}.");
        }

        [Fact]
        public async void ResetPassword_ShouldBeSuccessful()
        {
            var request = TestFactory.CreateHttpRequest(body: JObject.FromObject(CreateResetPasswordDto()));

            var result = await userFunction
                .ResetPassword(request)
                .ConfigureAwait(false);

            Assert.NotNull(result);
            Assert.True(result.GetType() == typeof(OkObjectResult), "Assert is OkObjectResult type.");

            var definition = (ResponseDto)((OkObjectResult)result).Value;

            Assert.NotNull(definition);
        }

        [Theory]
        [InlineData(false, "password", "677306e8-e411-4710-ba03-f10c23fed20b", ErrorCodesEnum.InvalidUpdatePasswordRequest)]
        [InlineData(true, null, "677306e8-e411-4710-ba03-f10c23fed20b", ErrorCodesEnum.InvalidUpdatePasswordRequestPassword)]
        [InlineData(true, "", "677306e8-e411-4710-ba03-f10c23fed20b", ErrorCodesEnum.InvalidUpdatePasswordRequestPassword)]
        [InlineData(true, " ", "677306e8-e411-4710-ba03-f10c23fed20b", ErrorCodesEnum.InvalidUpdatePasswordRequestPassword)]
        [InlineData(true, "password", null, ErrorCodesEnum.InvalidUpdatePasswordRequestResetPasswordCode)]
        [InlineData(true, "password", "", ErrorCodesEnum.InvalidUpdatePasswordRequestResetPasswordCode)]
        [InlineData(true, "password", " ", ErrorCodesEnum.InvalidUpdatePasswordRequestResetPasswordCode)]
        [InlineData(true, "password", "677306e8-e411-4710-ba03-f10c23fed20c", ErrorCodesEnum.UserNotFound)]
        public async void UpdatePassword_ShouldBeBadRequestAsync(bool hasDto, string password, string resetPasswordCode, ErrorCodesEnum error)
        {
            var request = TestFactory.CreateHttpRequest(body: hasDto ? JObject.FromObject(CreateUpdatePasswordDto(new List<Action<UpdatePasswordDto>>
                {
                    obj => obj.Password = password,
                    obj => obj.ResetPasswordCode = resetPasswordCode
                })) : null);

            var result = await userFunction
                .UpdatePassword(request)
                .ConfigureAwait(false);

            Assert.True(result.GetType() == typeof(BadRequestObjectResult), "Assert result is BadRequestObjectResult type.");

            var response = (ErrorDto)((BadRequestObjectResult)result).Value;

            Assertions.ErrorDefinitionIsNotNullOrEmpty(response);

            Assert.True(
                error.ToString().Equals(response.Code, StringComparison.InvariantCultureIgnoreCase),
                $"Assert ErrorDefinition.Code is {error}.");
        }

        [Fact]
        public async void UpdatePassword_ShouldBeSuccessful()
        {
            var request = TestFactory.CreateHttpRequest(body: JObject.FromObject(CreateUpdatePasswordDto()));

            var result = await userFunction
                .UpdatePassword(request)
                .ConfigureAwait(false);

            Assert.NotNull(result);
            Assert.True(result.GetType() == typeof(OkObjectResult), "Assert is OkObjectResult type.");

            var definition = (ResponseDto)((OkObjectResult)result).Value;

            Assert.NotNull(definition);
        }

        private static CreateUserDto CreateCreateUserDto(List<Action<CreateUserDto>> customizes = null)
        {
            var definition = new CreateUserDto
            {
                Email = "enrique@hotmail.com",
                Password = "1234567",
                LastName = "Gutierrez",
                Name = "Luis"
            };

            if (customizes != null)
            {
                foreach (var customize in customizes)
                {
                    customize?.Invoke(definition);
                }
            }

            return definition;
        }

        private static ResetPasswordDto CreateResetPasswordDto(List<Action<ResetPasswordDto>> customizes = null)
        {
            var definition = new ResetPasswordDto
            {
                Email = "alanfma@hotmail.com"
            };

            if (customizes != null)
            {
                foreach (var customize in customizes)
                {
                    customize?.Invoke(definition);
                }
            }

            return definition;
        }

        private static UpdatePasswordDto CreateUpdatePasswordDto(List<Action<UpdatePasswordDto>> customizes = null)
        {
            var definition = new UpdatePasswordDto
            {
                Password = "Password",
                ResetPasswordCode = TestDataFactory.resetPasswordCode
            };

            if (customizes != null)
            {
                foreach (var customize in customizes)
                {
                    customize?.Invoke(definition);
                }
            }

            return definition;
        }
    }
}
