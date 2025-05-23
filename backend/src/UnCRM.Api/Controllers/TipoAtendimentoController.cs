using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UnCRM.Api.Domain.Models;
using UnCRM.Api.Domain.Repository.Interfaces;

namespace UnCRM.Api.Controllers
{
    [ApiController]
    [Route("tipo-atendimento")]
    public class TipoAtendimentoController(IRepository<TipoAtendimento, long> repository) : ControllerBase
    {
        // <summary> Obtém todos os tipos de atendimento </summary>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(IEnumerable<TipoAtendimento>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObterTodos()
        {
            var tipos = await repository.Obter();
            return Ok(tipos);
        }
    }
}