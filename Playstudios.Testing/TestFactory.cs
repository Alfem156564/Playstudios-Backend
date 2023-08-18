namespace Playstudios.Testing
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Http.Internal;
    using Microsoft.Extensions.Primitives;
    using Newtonsoft.Json.Linq;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection.PortableExecutable;
    using System.Text;

    public class TestFactory
    {
        public static HttpRequest CreateHttpRequest(JObject? body = null, Dictionary<string, string> headers = null)
        {
            var context = new DefaultHttpContext();
            if(headers != null)
            {
                foreach(var header in headers)
                {
                    context.Request.Headers[header.Key] = header.Value;
                }
            }
            var request = context.Request;

            if (body != null)
            {
                byte[] byteArray = Encoding.ASCII.GetBytes(body.ToString());
                var stream = new MemoryStream(byteArray);

                request.Body = stream;
            }

            return request;
        }
    }
}
