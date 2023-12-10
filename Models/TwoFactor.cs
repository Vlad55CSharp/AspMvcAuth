using System.ComponentModel.DataAnnotations;
namespace AspMvcAuth.Models
{
    public class TwoFactor
    {
        [Required]
        public string TwoFactorCode { get; set; } = "";
        public string ReturnUrl { get; set; } = "/";
    }
}
