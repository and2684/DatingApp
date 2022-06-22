using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Entities
{
    [Table("Photos")]
    public class Photo
    {
        public int Id { get; set; }
        public string Url { get; set; } = string.Empty;

        public bool IsMain { get; set; }
        public string? PublicId { get; set; }
        [Required]
        public AppUser? AppUser { get; set; }
        public int AppUserId { get; set; }
    }
}