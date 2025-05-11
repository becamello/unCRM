using System.ComponentModel.DataAnnotations;
using UnCRM.Api.Domain.Enums;

namespace UnCRM.Api.Contract.Usuario
{
    public class UsuarioRequestContract : UsuarioLoginRequestContract
    {
        [Required]
        public required string Nome { get; set; }

        [Required]
        public CargoEnum Cargo { get; set; }
    }
}