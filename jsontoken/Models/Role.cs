using System.ComponentModel.DataAnnotations;

namespace jsontoken.Models
{
    public class Role
    {
        public int RoleId { get; set; }

        [Display(Name = "Role")]
        public string RoleName { get; set; }
    }
}
