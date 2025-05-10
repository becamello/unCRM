using System.ComponentModel.DataAnnotations;
using UnCRM.Api.Domain.Enums;

namespace UnCRM.Api.Domain.Models
{
    public class Usuario
    {
        [Key]
        public long Id { get; set; }

        [Required(ErrorMessage = "O campo de login é obrigatório.")]
        public string Login { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo de senha é obrigatório.")]
        public string Senha { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo de Nome é obrigatório.")]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O cargo do usuário é obrigatório.")]
        public CargoEnum? Cargo { get; set; }

        [Required]
        public DateTime DataCadastro { get; set; }
        public DateTime? DataInativacao { get; set; }
    }
}