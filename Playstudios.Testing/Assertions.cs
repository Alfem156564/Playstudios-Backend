namespace Playstudios.Testing
{
    using Playstudios.Common.Models.Dto;
    using Xunit;

    public class Assertions
    {
        public static void ErrorDefinitionIsNotNullOrEmpty(ErrorDto errorDto)
        {
            Assert.False(errorDto == null, "Assert ErrorDto is not null.");
            Assert.False(string.IsNullOrWhiteSpace(errorDto.Message), "Assert ErrorDto.Message is not null or empty.");
        }
    }
}
