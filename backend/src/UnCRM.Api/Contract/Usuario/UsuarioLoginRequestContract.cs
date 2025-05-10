using System.ComponentModel.DataAnnotations;

namespace UnCRM.Api.Contract.Usuario
{
    public class UsuarioLoginRequestContract
    {
        [Required(ErrorMessage = "O login é obrigatório.")]
        public string? Login { get; set; }

        [Required(ErrorMessage = "A senha é obrigatória.")]
        [MinLength(6, ErrorMessage = "A senha deve ter pelo menos 6 caracteres.")]
        public string? Senha { get; set; }
    }
}