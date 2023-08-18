namespace Playstudios.Common.Models.Dto
{
    using Newtonsoft.Json;

    [JsonObject]
    public class CreateUserDto : UserDto
    {
        [JsonProperty]
        public string Password { get; set; }
    }
}
