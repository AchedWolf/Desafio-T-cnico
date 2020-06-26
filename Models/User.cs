using System.ComponentModel.DataAnnotations;

namespace Luby.Models
{
    public class User
    {
        [Key]
        public int Id { get; private set; }

        [Required(ErrorMessage = "Este campo é obrigatório!")]
        public string user { get; set; }

        [Required(ErrorMessage = "Este campo é obrigatório!")]
        public string password { get; set; }
    }
}