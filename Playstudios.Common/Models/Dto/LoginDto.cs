namespace Playstudios.Common.Models.Dto
{
    using Newtonsoft.Json;

    [JsonObject]
    public class LoginDto
    {
        [JsonProperty]
        public string Email { get; set; }

        [JsonProperty]
        public string Password { get; set; }
    }
}
