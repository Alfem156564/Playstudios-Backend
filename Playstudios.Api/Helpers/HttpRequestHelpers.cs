namespace Playstudios.Api.Helpers
{
    using Microsoft.AspNetCore.Http;
    using Newtonsoft.Json;
    using System.IO;
    using System.Threading.Tasks;

    internal static class HttpRequestHelpers
    {
        public static async Task<T> GetRequestBodyAsync<T>(this HttpRequest request)
        {
            string requestBody = await new StreamReader(request.Body)
                .ReadToEndAsync()
                .ConfigureAwait(continueOnCapturedContext: false);

            return JsonConvert
                .DeserializeObject<T>(requestBody);
        }
    }
}
