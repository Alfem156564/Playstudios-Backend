using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs;
using Playstudios.Api.Helpers;
using Playstudios.Common.Models.Dto;
using Playstudios.Core.Contracts;
using System.Threading.Tasks;
using Playstudios.Api.Functions.Handlers;

namespace Playstudios.Api.Functions
{
    public class UserFunction
    {
        private readonly IUserManager userManager;

        public UserFunction(IUserManager _userManager)
        {
            userManager = _userManager;
        }

        [FunctionName("CreateUser")]
        public async Task<IActionResult> CreateUser(
            [HttpTrigger(
                AuthorizationLevel.Anonymous,
                "post",
                Route = ApiRoutes.USER)] HttpRequest request)
        {
            var userDto = await request
                .GetRequestBodyAsync<CreateUserDto>()
                .ConfigureAwait(continueOnCapturedContext: false);

            var validation = ValidationHelpers
                .GetUserDtoValidation(userDto);

            if (validation != null)
                return validation;

            var userResponse = await userManager
                .CreateUser(userDto.Name, userDto.LastName, userDto.Email, userDto.Password)
                .ConfigureAwait(false);

            validation = ValidationHelpers
               .GetCreateUserValidation(userResponse,
                userDto.Email);

            if (validation != null)
                return validation;

            return new OkObjectResult(
                userResponse.Value);
        }

        [FunctionName("ResetPassword")]
        public async Task<IActionResult> ResetPassword(
            [HttpTrigger(
                AuthorizationLevel.Anonymous,
                "post",
                Route = ApiRoutes.USER_RESET_PASSWORD)] HttpRequest request)
        {
            var resetPasswordDto = await request
                .GetRequestBodyAsync<ResetPasswordDto>()
                .ConfigureAwait(continueOnCapturedContext: false);

            var validation = ValidationHelpers
                .GetResetPasswordDtoValidation(resetPasswordDto);

            if (validation != null)
                return validation;

            var userResponse = await userManager
                .CreateResetPasswordCode(resetPasswordDto.Email)
                .ConfigureAwait(false);

            validation = ValidationHelpers
               .GetCreateResetPasswordCodeValidation(userResponse,
                resetPasswordDto.Email);

            if (validation != null)
                return validation;

            return new OkObjectResult(
                userResponse.Value);
        }

        [FunctionName("UpdatePassword")]
        public async Task<IActionResult> UpdatePassword(
            [HttpTrigger(
                AuthorizationLevel.Anonymous,
                "post",
                Route = ApiRoutes.USER_UPDATE_PASSWORD)] HttpRequest request)
        {
            var updatePasswordDto = await request
                .GetRequestBodyAsync<UpdatePasswordDto>()
                .ConfigureAwait(continueOnCapturedContext: false);

            var validation = ValidationHelpers
                .GetUpdatePasswordDtoValidation(updatePasswordDto);

            if (validation != null)
                return validation;

            var userResponse = await userManager
                .UpdatePasswordWithCode(updatePasswordDto.ResetPasswordCode, updatePasswordDto.Password)
                .ConfigureAwait(false);

            validation = ValidationHelpers
               .GetUpdatePasswordWithCodeValidation(userResponse);

            if (validation != null)
                return validation;

            return new OkObjectResult(
                userResponse.Value);
        }
    }
}
