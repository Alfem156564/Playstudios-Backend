namespace Playstudios.Common.Models.Dto
{
    using Newtonsoft.Json;

    [JsonObject]
    public class UserDto
    {
        [JsonProperty]
        public Guid? Id { get; set; }

        [JsonProperty]
        public string Name { get; set; }

        [JsonProperty]
        public string LastName { get; set; }

        [JsonProperty]
        public string Email { get; set; }
    }
}
