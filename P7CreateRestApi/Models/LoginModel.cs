using System.ComponentModel.DataAnnotations;

namespace P7CreateRestApi.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage="Le nom d'utilisateur est requis")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Le mot de passe est obligatoire.")]
        public string Password { get; set; }
    }
}
