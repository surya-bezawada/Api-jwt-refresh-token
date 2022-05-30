using jsontoken.Models;
using System.ComponentModel.DataAnnotations;

namespace jsontoken.Entities
{
    public class User
    {
        [Key]
        public  int UserId { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Mobile { get; set; }

        public string DOB { get; set; }

       // public RoleBased Role { get; set; }


        

    }
}
