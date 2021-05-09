using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Project.Web.Models.User
{
    public class UserWebModel
    {
        public Guid Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string MiddleName { get; set; }

        [Required]
        public string LastName { get; set; }

        public IList<string> Roles { get; set; }

        [Required]
        public string EGN { get; set; }

        [Required]
        public string PhoneNumber { get; set; }
    }
}
