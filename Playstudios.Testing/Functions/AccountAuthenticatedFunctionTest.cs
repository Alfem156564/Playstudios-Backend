namespace Playstudios.Testing.Functions
{
    using Microsoft.AspNetCore.Mvc;
    using Playstudios.Api.Functions.Authenticated;
    using Playstudios.Common.Enumerations;
    using Playstudios.Common.Models.Dto;
    using Playstudios.Testing.Functions.Base;
    using Xunit;

    public class AccountAuthenticatedFunctionTest : FunctionsTestBase
    {
        private AccountAuthenticatedFunction accountFunction;

        public AccountAuthenticatedFunctionTest()
        {
            databaseContextMock
                .Setup(context => context.Users)
                .Returns(TestDataFactory.GetUserMock().Object);

            databaseContextMock
                .Setup(context => context.Sessions)
                .Returns(TestDataFactory.GetSessionMock().Object);

            accountFunction = new AccountAuthenticatedFunction(accountManager);
        }

        [Theory]
        [InlineData(true, false, false, false, ErrorCodesEnum.InvalidLoginRequest)]
        [InlineData(false, true, false, false, ErrorCodesEnum.StringIsNotGuid)]
        [InlineData(false, false, true, false, ErrorCodesEnum.SessionNotFound)]
        [InlineData(false, false, false, true, ErrorCodesEnum.SessionExpired)]
        public async void GetSessionUser_ShouldBeBadRequestAsync(bool removeHeader, bool invalidGuid, bool sessionNotFound, bool expiredSession, ErrorCodesEnum error)
        {
            if (removeHeader)
                Headers.Remove(AUTHORIZATIONHEADER);
            if (invalidGuid)
                Headers[AUTHORIZATIONHEADER] = "INVALIDGUID";
            if (sessionNotFound)
                Headers[AUTHORIZATIONHEADER] = Guid.NewGuid().ToString();
            if (expiredSession)
                Headers[AUTHORIZATIONHEADER] = TestDataFactory.expiredSessionId.ToString();

            var request = TestFactory.CreateHttpRequest(headers:Headers);

            var result = await accountFunction
                .GetSessionUser(request)
                .ConfigureAwait(false);

            Assert.True(result.GetType() == typeof(BadRequestObjectResult), "Assert result is BadRequestObjectResult type.");

            var response = (ErrorDto)((BadRequestObjectResult)result).Value;

            Assertions.ErrorDefinitionIsNotNullOrEmpty(response);

            Assert.True(
                error.ToString().Equals(response.Code, StringComparison.InvariantCultureIgnoreCase),
                $"Assert ErrorDefinition.Code is {error}.");
        }

        [Fact]
        public async void GetSessionUser_ShouldBeSuccessful()
        {
            var request = TestFactory.CreateHttpRequest(headers: Headers);

            var result = await accountFunction
                .GetSessionUser(request)
                .ConfigureAwait(false);

            Assert.NotNull(result);
            Assert.True(result.GetType() == typeof(OkObjectResult), "Assert is OkObjectResult type.");

            var definition = (UserDto)((OkObjectResult)result).Value;

            Assert.NotNull(definition);
        }

        private const string AUTHORIZATIONHEADER = "Authorization";

        private Dictionary<string, string> Headers = new Dictionary<string, string>
        {
            {AUTHORIZATIONHEADER, TestDataFactory.activeSessionId.ToString()}
        };
    }
}
