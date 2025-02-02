using System.ComponentModel.DataAnnotations;

    namespace WebApiLivro.Models
    {
        public class Usuario
        {
            [Key]
            public int Id { get; set; }

            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            public string SenhaHash { get; set; }

            public string Nome { get; set; }
        }
    }

