namespace Playstudios.Data.Models.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Session")]
    public class SessionEntity
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [Column("UserId")]
        public Guid UserEntityId { get; set; }

        [Required]
        public DateTime ExpirationDate { get; set; } = DateTime.UtcNow.AddDays(10);

        public UserEntity User { get; set; }
    }
}
