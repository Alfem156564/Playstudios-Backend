namespace Playstudios.Api.Functions.Authenticated
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Routing;
    using Microsoft.Azure.WebJobs.Extensions.Http;
    using Microsoft.Azure.WebJobs;
    using Playstudios.Api.Functions.Handlers;
    using Playstudios.Core.Contracts;
    using System.Threading.Tasks;

    public class AccountAuthenticatedFunction : CheckAuthenticatedMiddleware
    {
        public AccountAuthenticatedFunction(IAccountManager _accountManager)
            :base(_accountManager)
        {
        }

        [FunctionName("GetSessionUser")]
        public async Task<IActionResult> GetSessionUser(
            [HttpTrigger(
                AuthorizationLevel.Anonymous,
                "get",
                Route = ApiRoutes.ACCOUNT_ME)] HttpRequest request)
        {
            var validation = ValidateUserHasSession(request);

            if (validation != null)
                return validation;

            return new OkObjectResult(GetCurrentUser());
        }
    }
}
