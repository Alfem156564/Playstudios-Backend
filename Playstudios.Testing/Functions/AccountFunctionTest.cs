namespace Playstudios.Testing.Functions
{
    using Microsoft.AspNetCore.Mvc;
    using Newtonsoft.Json.Linq;
    using Playstudios.Api.Functions;
    using Playstudios.Common.Enumerations;
    using Playstudios.Common.Models.Dto;
    using Playstudios.Testing.Functions.Base;
    using Xunit;

    public class AccountFunctionTest : FunctionsTestBase
    {
        private AccountFunction accountFunction;

        public AccountFunctionTest()
        {
            databaseContextMock
                .Setup(context => context.Users)
                .Returns(TestDataFactory.GetUserMock().Object);

            databaseContextMock
                .Setup(context => context.Sessions)
                .Returns(TestDataFactory.GetSessionMock().Object);

            accountFunction = new AccountFunction(accountManager);
        }

        [Theory]
        [InlineData(false, "darciaitachi@hotmail.com", "1234567", ErrorCodesEnum.InvalidLoginRequest)]
        [InlineData(true, null, "1234567", ErrorCodesEnum.InvalidLoginRequestEmail)]
        [InlineData(true, "", "1234567", ErrorCodesEnum.InvalidLoginRequestEmail)]
        [InlineData(true, " ", "1234567", ErrorCodesEnum.InvalidLoginRequestEmail)]
        [InlineData(true, "darciaitachihotmail.com", "1234567", ErrorCodesEnum.InvalidLoginRequestEmailFormat)]
        [InlineData(true, "darciaitachi@hotmail.com", null, ErrorCodesEnum.InvalidLoginRequestPassword)]
        [InlineData(true, "darciaitachi@hotmail.com", "", ErrorCodesEnum.InvalidLoginRequestPassword)]
        [InlineData(true, "darciaitachi@hotmail.com", " ", ErrorCodesEnum.InvalidLoginRequestPassword)]
        public async void Login_ShouldBeBadRequestAsync(bool hasDto, string email, string password, ErrorCodesEnum error)
        {
            var request = TestFactory.CreateHttpRequest(body: hasDto ? JObject.FromObject(CreateLoginDto(new List<Action<LoginDto>>
                {
                    obj => obj.Email = email,
                    obj => obj.Password = password,
                })) : null);

            var result = await accountFunction
                .Login(request)
                .ConfigureAwait(false);

            Assert.True(result.GetType() == typeof(BadRequestObjectResult), "Assert result is BadRequestObjectResult type.");

            var response = (ErrorDto)((BadRequestObjectResult)result).Value;

            Assertions.ErrorDefinitionIsNotNullOrEmpty(response);

            Assert.True(
                error.ToString().Equals(response.Code, StringComparison.InvariantCultureIgnoreCase),
                $"Assert ErrorDefinition.Code is {error}.");
        }

        [Fact]
        public async void Login_ShouldBeSuccessful()
        {
            var request = TestFactory.CreateHttpRequest(body: JObject.FromObject(CreateLoginDto()));

            var result = await accountFunction
                .Login(request)
                .ConfigureAwait(false);

            Assert.NotNull(result);
            Assert.True(result.GetType() == typeof(OkObjectResult), "Assert is OkObjectResult type.");

            var definition = (SessionDto)((OkObjectResult)result).Value;

            Assert.NotNull(definition);
        }

        private static LoginDto CreateLoginDto(List<Action<LoginDto>> customizes = null)
        {
            var definition = new LoginDto
            {
                Email = "darciaitachi@hotmail.com",
                Password = "1234567"
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
