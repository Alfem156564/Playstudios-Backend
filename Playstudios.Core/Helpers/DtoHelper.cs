namespace Playstudios.Core.Helpers
{
    using Playstudios.Common.Models.Dto;
    using Playstudios.Data.Models.Entities;

    public static class DtoHelper
    {
        public static UserDto? ToDto(this UserEntity? entity) => (entity == null) ? 
            null : 
            new UserDto
            {
                Email = entity.Email,
                Id = entity.Id,
                LastName = entity.LastName,
                Name = entity.Name
            };
    }
}
