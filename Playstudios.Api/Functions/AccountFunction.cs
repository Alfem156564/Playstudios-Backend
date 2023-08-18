using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs;
using Playstudios.Core.Contracts;
using System.Threading.Tasks;
using Playstudios.Api.Helpers;
using Playstudios.Common.Models.Dto;
using Playstudios.Core.Managers;
using Playstudios.Api.Functions.Handlers;

namespace Playstudios.Api.Functions
{
    public class AccountFunction
    {
        private readonly IAccountManager accountManager;

        public AccountFunction(IAccountManager _accountManager)
        {
            accountManager = _accountManager;
        }

        [FunctionName("Login")]
        public async Task<IActionResult> Login(
            [HttpTrigger(
                AuthorizationLevel.Anonymous,
                "post",
                Route = ApiRoutes.LOGIN)] HttpRequest request)
        {
            var loginDto = await request
                .GetRequestBodyAsync<LoginDto>()
                .ConfigureAwait(continueOnCapturedContext: false);

            var validation = ValidationHelpers
                .GetLoginDtoValidation(loginDto);

            if (validation != null)
                return validation;

            var userResponse = await accountManager
                .CreateUserSession(loginDto.Email, loginDto.Password)
                .ConfigureAwait(false);

            validation = ValidationHelpers
               .GetUserByEmailAndPasswordValidation(userResponse,
                loginDto.Email);

            if (validation != null)
                return validation;

            return new OkObjectResult(
                userResponse.Value);
        }
    }
}
