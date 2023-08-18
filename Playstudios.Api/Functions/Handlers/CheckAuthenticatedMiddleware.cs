namespace Playstudios.Api.Functions.Handlers
{
    using Playstudios.Common.Models.Dto;
    using Playstudios.Core.Contracts;
    using Playstudios.Api.Helpers;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using System;

    public class CheckAuthenticatedMiddleware
    {
        private UserDto user;

        private IAccountManager accountManager;

        private const string AUTHORIZATIONHEADERKEY = "Authorization";

        public CheckAuthenticatedMiddleware(IAccountManager _accountManager)
        {
            accountManager = _accountManager;
        }

        protected IActionResult ValidateUserHasSession(HttpRequest request)
        {
            var validation = ValidationHelpers
               .ValidateRequestHeaderKey(
                   message: request,
                   key: AUTHORIZATIONHEADERKEY);

            if (validation != null)
                return validation;

            string id = request.Headers[AUTHORIZATIONHEADERKEY]
                .ToString();

            validation = ValidationHelpers
               .GetStringParseGuidValidation(
                   id);

            if (validation != null)
                return validation;

            var accountResponse = accountManager
                    .GetUserSession(Guid.Parse(id));

            validation = ValidationHelpers
               .GetLoggedUserValidation(accountResponse);

            if (validation != null)
                return validation;

            user = accountResponse.Value;

            return null;
        }

        protected UserDto GetCurrentUser() => user;
    }
}
