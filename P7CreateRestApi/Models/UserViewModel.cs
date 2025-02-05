using System.ComponentModel.DataAnnotations;

namespace Dot.Net.WebApi.Model
{
    public class UserViewModel
    {
        public string Username { get; set; }
        [Required(ErrorMessage = "Le mot de passe est obligatoire.")]
        [RegularExpression(@"^(?=.*[A-Z])(?=.*\d)(?=.*[^A-Za-z0-9]).{8,}$",
        ErrorMessage = "Le mot de passe doit contenir au moins 8 caractères, une lettre majuscule, un chiffre et un symbole.")]
        public string Password { get; set; }
        public string Fullname { get; set; }
        public string Role { get; set; }
    }
}