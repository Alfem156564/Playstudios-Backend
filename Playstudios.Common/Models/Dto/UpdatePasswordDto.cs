namespace Playstudios.Common.Models.Dto
{
    using Newtonsoft.Json;

    [JsonObject]
    public class UpdatePasswordDto
    {
        [JsonProperty]
        public string Password { get; set; }

        [JsonProperty]
        public string ResetPasswordCode { get; set; }
    }
}
