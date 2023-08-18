namespace Playstudios.Common.Models.Dto
{
    using Newtonsoft.Json;

    [JsonObject]
    public class ResponseDto
    {
        [JsonProperty]
        public string Message { get; set; }
    }
}
