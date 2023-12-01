using System.ComponentModel.DataAnnotations;

namespace APIControlNet.DTOs
{
    public class CredencialesUsuariosDTO
    { 
        public string Id { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public bool EmailConfirmed { get; set; }
        //[Required]
        public string Password { get; set; }
    }
}
