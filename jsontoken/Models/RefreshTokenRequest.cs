using System.ComponentModel.DataAnnotations;

namespace jsontoken.Models
{
    public class RefreshTokenRequest
    {
        [Required]
        public string ExpiredToken { get; set; }
        [Required]
        public string RefreshToken { get; set; }
    }
}
