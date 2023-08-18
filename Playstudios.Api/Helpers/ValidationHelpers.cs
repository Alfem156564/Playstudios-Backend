namespace Playstudios.Api.Helpers
{
    using Microsoft.AspNetCore.Mvc;
    using Playstudios.Common.Enumerations;
    using Playstudios.Common.Models.Dto;
    using System.Globalization;
    using System.Text.RegularExpressions;
    using System;
    using Playstudios.Common.Models;
    using Microsoft.Azure.WebJobs.Host;
    using System.Linq;
    using Microsoft.AspNetCore.Http;

    public static class ValidationHelpers
    {
        #region Account
        public static IActionResult GetLoginDtoValidation(LoginDto loginDto)
        {
            var validator = new Validator<LoginDto>(loginDto)
                .AddBadRequestValidation(request =>
                    request == null,
                    ErrorCodesEnum.InvalidLoginRequest,
                    "The login request can't be null.")
                .AddBadRequestValidation(request =>
                    string.IsNullOrWhiteSpace(request.Email),
                    ErrorCodesEnum.InvalidLoginRequestEmail,
                    "The login request email can't be null or empty.")
                .AddBadRequestValidation(request =>
                    !IsValidEmail(request.Email),
                    ErrorCodesEnum.InvalidLoginRequestEmailFormat,
                    "The login request email must have a valid format.")
                .AddBadRequestValidation(request =>
                    string.IsNullOrWhiteSpace(request.Password),
                    ErrorCodesEnum.InvalidLoginRequestPassword,
                    "The login request password can't be null or empty.");

            return validator.Validate();
        }

        public static IActionResult ValidateRequestHeaderKey(HttpRequest message, 
            string key)
        {
            var validator = new Validator<HttpRequest>(message)
                .AddBadRequestValidation(definition =>
                    message == null,
                    ErrorCodesEnum.NullRequestMessage,
                    "The request can't be null")
                .AddBadRequestValidation(definition =>
                    !message.Headers.ContainsKey(key),
                    ErrorCodesEnum.InvalidLoginRequest,
                    $"The headers doesn't contains {key}");

            return validator.Validate();
        }

        public static IActionResult GetUserByEmailAndPasswordValidation(ManagerResult<SessionDto> userResponse,
            string email)
        {
            if (!userResponse.DidSucceed)
            {
                var validator = new Validator<ManagerResult<SessionDto>>(userResponse)
                    .AddBadRequestValidation(response =>
                        response.ErrorCode == ErrorCodesEnum.UserEmailNotFound,
                        ErrorCodesEnum.UserEmailNotFound,
                        $"The user with email {email} was not found.")
                    .AddBadRequestValidation(response =>
                        response.ErrorCode == ErrorCodesEnum.UserPasswordNotFound,
                        ErrorCodesEnum.UserPasswordNotFound,
                        $"The password is invalid.")
                    .AddUnprocessableEntityValidation(request =>
                        !request.DidSucceed,
                        ErrorCodesEnum.Unknown,
                        "There was a unknown problem, please try again later.");

                return validator.Validate();
            }
            return null;
        }

        public static IActionResult GetLoggedUserValidation(ManagerResult<UserDto> userResponse)
        {
            if (!userResponse.DidSucceed)
            { 
                var validator = new Validator<ManagerResult<UserDto>>(userResponse)
                    .AddBadRequestValidation(response =>
                        response.ErrorCode == ErrorCodesEnum.SessionNotFound,
                        ErrorCodesEnum.SessionNotFound,
                        $"The session wasn't found.")
                    .AddBadRequestValidation(response =>
                        response.ErrorCode == ErrorCodesEnum.SessionExpired,
                        ErrorCodesEnum.SessionExpired,
                        $"The session has already expired, plase login again.")
                    .AddUnprocessableEntityValidation(request =>
                        !request.DidSucceed,
                        ErrorCodesEnum.Unknown,
                        "There was a unknown problem, please try again later.");

                return validator.Validate();
            }
            return null;
        }

        public static IActionResult GetStringParseGuidValidation(string id)
        {
            var validator = new Validator<string>(id)
                .AddBadRequestValidation(response =>
                    !Guid.TryParse(response, out Guid r),
                    ErrorCodesEnum.StringIsNotGuid,
                    $"The session token has an invalid format.");

            return validator.Validate();
        }
        #endregion

        #region User
        public static IActionResult GetUserDtoValidation(CreateUserDto userDto)
        {
            var validator = new Validator<CreateUserDto>(userDto)
                .AddBadRequestValidation(request =>
                    request == null,
                    ErrorCodesEnum.InvalidUserRequest,
                    "The user request can't be null.")
                .AddBadRequestValidation(request =>
                    string.IsNullOrWhiteSpace(request.Name),
                    ErrorCodesEnum.InvalidUserRequestName,
                    "The user request name can't be null or empty.")
                .AddBadRequestValidation(request =>
                    string.IsNullOrWhiteSpace(request.LastName),
                    ErrorCodesEnum.InvalidUserRequestLastName,
                    "The user request last name can't be null or empty.")
                .AddBadRequestValidation(request =>
                    string.IsNullOrWhiteSpace(request.Email),
                    ErrorCodesEnum.InvalidUserRequestEmail,
                    "The user request email can't be null or empty.")
                .AddBadRequestValidation(request =>
                    !IsValidEmail(request.Email),
                    ErrorCodesEnum.InvalidUserRequestEmailFormat,
                    "The user request email must have a valid format.")
                .AddBadRequestValidation(request =>
                    string.IsNullOrWhiteSpace(request.Password),
                    ErrorCodesEnum.InvalidUserRequestPassword,
                    "The user request password can't be null or empty.");

            return validator.Validate();
        }

        public static IActionResult GetResetPasswordDtoValidation(ResetPasswordDto resetPasswordDto)
        {
            var validator = new Validator<ResetPasswordDto>(resetPasswordDto)
                .AddBadRequestValidation(request =>
                    request == null,
                    ErrorCodesEnum.InvalidResetPasswordRequest,
                    "The reset password request can't be null.")
                .AddBadRequestValidation(request =>
                    string.IsNullOrWhiteSpace(request.Email),
                    ErrorCodesEnum.InvalidResetPasswordRequestEmail,
                    "The reset password request email can't be null or empty.")
                .AddBadRequestValidation(request =>
                    !IsValidEmail(request.Email),
                    ErrorCodesEnum.InvalidResetPasswordRequestEmailFormat,
                    "The reset password request email must have a valid format.");

            return validator.Validate();
        }

        public static IActionResult GetUpdatePasswordDtoValidation(UpdatePasswordDto updatePasswordDto)
        {
            var validator = new Validator<UpdatePasswordDto>(updatePasswordDto)
                .AddBadRequestValidation(request =>
                    request == null,
                    ErrorCodesEnum.InvalidResetPasswordRequest,
                    "The update password request can't be null.")
                .AddBadRequestValidation(request =>
                    string.IsNullOrWhiteSpace(request.Password),
                    ErrorCodesEnum.InvalidResetPasswordRequestEmail,
                    "The update password request password can't be null or empty.")
                .AddBadRequestValidation(request =>
                    string.IsNullOrWhiteSpace(request.ResetPasswordCode),
                    ErrorCodesEnum.InvalidResetPasswordRequestEmailFormat,
                    "The update password request reset code can't be null or empty.");

            return validator.Validate();
        }

        public static IActionResult GetCreateUserValidation(ManagerResult<UserDto> userResponse,
            string email)
        {
            if (!userResponse.DidSucceed)
            {
                var validator = new Validator<ManagerResult<UserDto>>(userResponse)
                    .AddBadRequestValidation(response =>
                        response.ErrorCode == ErrorCodesEnum.UserEmailAllreadyUsed,
                        ErrorCodesEnum.UserEmailAllreadyUsed,
                        $"The user with email {email} already exists.")
                    .AddUnprocessableEntityValidation(request =>
                        !request.DidSucceed,
                        ErrorCodesEnum.Unknown,
                        "There was a unknown problem, please try again later.");

                return validator.Validate();
            }
            return null;
        }

        public static IActionResult GetCreateResetPasswordCodeValidation(ManagerResult<ResponseDto> userResponse,
            string email)
        {
            if (!userResponse.DidSucceed)
            {
                var validator = new Validator<ManagerResult<ResponseDto>>(userResponse)
                    .AddBadRequestValidation(response =>
                        response.ErrorCode == ErrorCodesEnum.UserEmailNotFound,
                        ErrorCodesEnum.UserEmailNotFound,
                        $"The user with email {email} was not found.")
                    .AddBadRequestValidation(response =>
                        response.ErrorCode == ErrorCodesEnum.SendEmailError,
                        ErrorCodesEnum.SendEmailError,
                        $"There was a problem sending the email to {email}.")
                    .AddUnprocessableEntityValidation(request =>
                        !request.DidSucceed,
                        ErrorCodesEnum.Unknown,
                        "There was a unknown problem, please try again later.");

                return validator.Validate();
            }
            return null;
        }

        public static IActionResult GetUpdatePasswordWithCodeValidation(ManagerResult<ResponseDto> userResponse)
        {
            if (!userResponse.DidSucceed)
            {
                var validator = new Validator<ManagerResult<ResponseDto>>(userResponse)
                    .AddBadRequestValidation(response =>
                        response.ErrorCode == ErrorCodesEnum.UserNotFound,
                        ErrorCodesEnum.UserNotFound,
                        $"The user was not found.")
                    .AddUnprocessableEntityValidation(request =>
                        !request.DidSucceed,
                        ErrorCodesEnum.Unknown,
                        "There was a unknown problem, please try again later.");

                return validator.Validate();
            }
            return null;
        }
        #endregion

        private static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            try
            {
                // Normalize the domain
                email = Regex.Replace(email, @"(@)(.+)$", DomainMapper,
                                      RegexOptions.None, TimeSpan.FromMilliseconds(200));

                // Examines the domain part of the email and normalizes it.
                string DomainMapper(Match match)
                {
                    // Use IdnMapping class to convert Unicode domain names.
                    var idn = new IdnMapping();

                    // Pull out and process domain name (throws ArgumentException on invalid)
                    string domainName = idn.GetAscii(match.Groups[2].Value);

                    return match.Groups[1].Value + domainName;
                }
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
            catch (ArgumentException)
            {
                return false;
            }

            try
            {
                return Regex.IsMatch(email,
                    @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                    RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            }
            catch (RegexMatchTimeoutException)
            {
                return false;
            }
        }
    }
}
