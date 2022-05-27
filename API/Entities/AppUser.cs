using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Entities
{
    public class AppUser
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public string UserName { get; set; }
    }
}