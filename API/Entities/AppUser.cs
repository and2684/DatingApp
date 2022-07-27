using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using API.Extensions;

namespace API.Entities
{
    public class AppUser
    {
        public int Id { get; set; }
        public string? Username { get; set; } = string.Empty;
        public byte[]? PasswordHash { get; set; }
        public byte[]? PasswordSalt { get; set; }
        public DateTime DateOfBirth {get; set;}
        public string? KnownAs { get; set; } = string.Empty;
        public DateTime? Created { get; set; } = DateTime.Now;
        public DateTime? LastActive { get; set; } = DateTime.Now;
        public string? Gender { get; set; } = string.Empty;
        public string? Introduction { get; set; } = string.Empty;
        public string? LookingFor { get; set; } = string.Empty;
        public string? Interests { get; set; } = string.Empty;
        public string? City { get; set; } = string.Empty;
        public string? Country { get; set; } = string.Empty;
        public ICollection<Photo>? Photos { get; set; }

        // public int GetAge()
        // {
        //     return DateOfBirth.CalculateAge();
        // }
    }
}