using System.ComponentModel.DataAnnotations;

namespace Authentication.Models.Dtos
{
    public class TokenRequestDto
    {
        [Required]
        public string Token { get; set; }
        [Required]
        public string RefreshToken { get; set; }
    }
}
