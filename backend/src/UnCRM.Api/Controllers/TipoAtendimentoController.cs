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
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ObterTodos()
        {
            var tipos = await repository.Obter();
            return Ok(tipos);
        }
    }
}