using System.ComponentModel.DataAnnotations;

namespace API.Entities
{
    public class UserLike
    {        
        public AppUser? SourceUser { get; set; }
        [Required]        
        public int SourceUserId { get; set; }
        public AppUser? LikedUser { get; set; }
        [Required]        
        public int LikedUserId { get; set; }
        
    }
}