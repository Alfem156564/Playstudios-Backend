namespace Playstudios.Common.Models.Dto
{
    using Newtonsoft.Json;

    [JsonObject]
    public class ResetPasswordDto
    {
        [JsonProperty]
        public string Email { get; set; }
    }
}
