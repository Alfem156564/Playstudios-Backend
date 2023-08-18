namespace Playstudios.Common.Models.Dto
{
    using Newtonsoft.Json;

    [JsonObject]
    public class SessionDto
    {
        [JsonProperty]
        public Guid Id { get; set; }

        [JsonProperty]
        public UserDto User { get; set; }
    }
}
