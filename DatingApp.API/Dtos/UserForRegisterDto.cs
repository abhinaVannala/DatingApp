using System;
using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.Dtos
{
    public class UserForRegisterDto
    {
        [Required]
        public string Username { get; set; }

        [Required]
        [StringLength(8, MinimumLength=4, ErrorMessage  = "you must specify password between 4 and 8 charecters")]

        
        public string Password { get; set; }

        [Required]
        public string Gender { get; set; }

        [Required]
        public string KnownAs { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string city { get; set; }

        [Required]
        public DateTime Created { get; set; }

        [Required]
        public string country { get; set; }
        public DateTime LastActive { get; set; }

        public UserForRegisterDto(){
           Created = DateTime.Now;
           LastActive = DateTime.Now;
        }
    }
}