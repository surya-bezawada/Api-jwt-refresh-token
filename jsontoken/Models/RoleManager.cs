using jsontoken.Entities;
using System.ComponentModel.DataAnnotations;

namespace jsontoken.Models
{
    public class RoleManager
    {
        public int Id { get; set; }

        public User User { get; set; }

        [Required]
        [Display(Name = "User")]
        public int UserId { get; set; }

        public Role Role { get; set; }

        [Required]
        [Display(Name = "Role")]
        public int RoleId { get; set; }
    }
}
