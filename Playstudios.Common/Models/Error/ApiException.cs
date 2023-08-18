namespace Playstudios.Common.Models.Error
{
    using Playstudios.Common.Enumerations;
    using System;
    using System.Net;

    public class ApiException : Exception
    {
        public ErrorCodesEnum ErrorCode { get; set; }

        public string ErrorMessage { get; set; }

        public HttpStatusCode StatusCode { get; set; }
    }
}
