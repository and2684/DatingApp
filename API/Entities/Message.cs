using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public string SenderUsername { get; set; } = string.Empty;
        public AppUser Sender { get; set; } = new AppUser();
        public int RecipientId { get; set; }
        public string RecipientUsername { get; set; } = string.Empty;
        public AppUser Recipient { get; set; } = new AppUser();
        public DateTime? DateRead { get; set; }
        public DateTime MessageSent { get; set; } = DateTime.Now;
        public bool SenderDeleted { get; set; }
        public bool RecipientDeleted { get; set; }
        public string Content { get; set; } = string.Empty;
    }
}