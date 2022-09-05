using API.Entities;

namespace API.DTOs
{
    public class MessageDto
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public string SenderUsername { get; set; } = string.Empty;
        public string SenderPhotoUrl { get; set; } = string.Empty;
        public int RecipientId { get; set; }
        public string RecipientUsername { get; set; } = string.Empty;
        public string RecipientPhotoUrl { get; set; } = string.Empty;
        public DateTime? DateRead { get; set; }
        public DateTime MessageSent { get; set; }
    }
}