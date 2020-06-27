using System.ComponentModel.DataAnnotations;

namespace Luby.Models
{
    public class Task
    {
        [Key]
        public int Id { get; private set; }

        public string description { get; set; }

        public bool concluded { get; set; } 

        [Required(ErrorMessage = "Este campo é obrigatório.")]
        public int UserId { get; set; }
    }
}