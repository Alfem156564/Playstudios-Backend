using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Playstudios.Common.Models.Dto
{
    using Newtonsoft.Json;

    [JsonObject]
    public class ErrorDto
    {
        [JsonProperty]
        public string Code { get; set; }

        [JsonProperty]
        public string Message { get; set; }
    }
}
