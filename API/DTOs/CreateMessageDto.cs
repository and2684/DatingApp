using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.DTOs
{
    public class CreateMessageDto
    {
        public string RecipientUsername { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
    }
}