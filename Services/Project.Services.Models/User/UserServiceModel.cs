using System;
using System.Collections.Generic;

namespace Project.Services.Models.User
{
    public class UserServiceModel
    {
        public Guid Id { get; set; }

        public IList<string> Roles { get; set; }

        public string Username { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string MiddleName { get; set; }

        public string LastName { get; set; }

        public string EGN { get; set; }

        public string PhoneNumber { get; set; }
    }
}
