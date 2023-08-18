namespace Playstudios.Api.Helpers
{
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System;
    using System.Linq;
    using Playstudios.Common.Enumerations;
    using Playstudios.Common.Models.Dto;
    using Playstudios.Common.Models.Error;
    using System.Net;

    public class Validator<T>
    {
        private readonly T request;

        private readonly List<Tuple<Func<T, bool>, ErrorCodesEnum, string, Func<object, IActionResult>>> validations =
            new List<Tuple<Func<T, bool>, ErrorCodesEnum, string, Func<object, IActionResult>>>();

        private readonly List<Func<IActionResult>> validators = new List<Func<IActionResult>>();

        public Validator(T request)
        {
            this.request = request;
        }

        public Validator<T> AddValidator(Func<IActionResult> validator)
        {
            validators.Add(validator);
            return this;
        }

        public Validator<T> AddBadRequestValidation(Func<T, bool> isInvalid, ErrorCodesEnum errorCode, string message)
        {
            return AddValidation(isInvalid, errorCode, message, error => new BadRequestObjectResult(error));
        }

        public Validator<T> AddConflictValidation(Func<T, bool> isInvalid, ErrorCodesEnum errorCode, string message)
        {
            return AddValidation(isInvalid, errorCode, message, error => new ConflictObjectResult(error));
        }

        public Validator<T> AddUnprocessableEntityValidation(Func<T, bool> isInvalid, ErrorCodesEnum errorCode, string message)
        {
            return AddValidation(isInvalid, errorCode, message, error => new UnprocessableEntityObjectResult(error));
        }

        public Validator<T> AddNotFoundValidation(Func<T, bool> isInvalid, ErrorCodesEnum errorCode, string message)
        {
            return AddValidation(isInvalid, errorCode, message, error => new NotFoundObjectResult(error));
        }

        public Validator<T> AddBadGatewayValidation(Func<T, bool> isInvalid, ErrorCodesEnum errorCode, string message)
        {
            return AddValidation(isInvalid, errorCode, message, error => new ObjectResult(error)
            {
                StatusCode = 502
            });
        }

        public Validator<T> AddForbiddenValidation(Func<T, bool> isInvalid, ErrorCodesEnum errorCode, string message)
        {
            return AddValidation(isInvalid, errorCode, message, error => new ObjectResult(error)
            {
                StatusCode = 403
            });
        }

        private Validator<T> AddValidation(Func<T, bool> isInvalid, ErrorCodesEnum errorCode, string message, Func<object, IActionResult> createResult)
        {
            validations.Add(new Tuple<Func<T, bool>, ErrorCodesEnum, string, Func<object, IActionResult>>(isInvalid, errorCode, message, createResult));
            return this;
        }

        public IActionResult Validate()
        {
            var validationsResult = validations
                .Where(validation => validation.Item1(request))
                .Select(validation =>
                {
                    var error = new ErrorDto
                    {
                        Code = validation.Item2.ToString(),
                        Message = validation.Item3
                    };

                    return validation.Item4(error);
                })
                .FirstOrDefault();

            if (validationsResult != null) return validationsResult;

            return validators
               .Select(validator => validator.Invoke())
               .FirstOrDefault(validatorResult => validatorResult != null);
        }
    }
}
