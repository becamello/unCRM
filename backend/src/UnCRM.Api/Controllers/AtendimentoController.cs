using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UnCRM.Api.Contract.Atendimento;
using UnCRM.Api.Contract.Atendimento.Request;
using UnCRM.Api.Domain.Services.Classes;

namespace UnCRM.Api.Controllers
{
    [ApiController]
    [Route("atendimento")]
    public class AtendimentoController(AtendimentoService service) : BaseController
    {
        private readonly AtendimentoService _service = service;

        // <summary> Cria um novo atendimento com parecer inicial.</summary>
        [HttpPost]
        [ProducesResponseType(typeof(AtendimentoCriarResponseContract), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Authorize]
        public async Task<IActionResult> Criar([FromBody] AtendimentoCriarRequestContract request)
        {
            var usuarioLogadoId = ObterIdUsuarioLogado();
            return Created("", await _service.Criar(request, usuarioLogadoId));
        }

        /// <summary> Registra um parecer no atendimento.</summary>
        [HttpPost("/parecer/{id}")]
        [Authorize]
        [ProducesResponseType(typeof(AtendimentoParecerResponseContract), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RegistrarParecer(long id, [FromBody] AtendimentoRegistrarParecerRequestContract request)
        {
            var usuarioLogadoId = ObterIdUsuarioLogado();
            return Created("", await _service.RegistrarParecer(id, usuarioLogadoId, request));
        }

        /// <summary> Obtém todos os atendimentos com base em filtros.</summary>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(IEnumerable<AtendimentoResponseContract>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObterTodos([FromQuery] AtendimentoQueryRequestContract filtro)
        {
            return Ok(await _service.ObterTodos(filtro));
        }

        /// <summary> Obtém um atendimento por ID.</summary>
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(AtendimentoResponseContract), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> ObterPorId(long id)
        {
            return Ok(await _service.ObterPorId(id));
        }

        /// <summary> Atualiza um atendimento.</summary>
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Atualizar(long id, [FromBody] AtendimentoCriarRequestContract request)
        {
            var usuarioLogadoId = ObterIdUsuarioLogado();
            await _service.Atualizar(id, usuarioLogadoId, request);
            return NoContent();
        }

        /// <summary> Edita um parecer dentro de um atendimento.</summary>
        [HttpPut("{id}/parecer/{parecerId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> EditarParecer(long id, long parecerId, [FromBody] AtendimentoEditarParecerRequestContract request)
        {
            var usuarioLogadoId = ObterIdUsuarioLogado();
            await _service.EditarParecer(id, parecerId, request, usuarioLogadoId);
            return NoContent();
        }

        /// <summary> Reabre um atendimento encerrado.</summary>
        [HttpPut("{id}/reabrir")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Reabrir(long id)
        {
            var usuarioLogadoId = ObterIdUsuarioLogado();
            await _service.Reabrir(id, usuarioLogadoId);
            return NoContent();
        }

        /// <summary> Encerra um atendimento isoladamente.</summary>
        [HttpPut("{id}/encerrar")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Encerrar(long id)
        {
            await _service.Encerrar(id);
            return NoContent();
        }

        /// <summary> Registra um próximo contato a um atendimento isoladamente.</summary>
        [HttpPut("{id}/proximo-contato")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RegistrarProximoContato(long id, [FromBody] AtendimentoRegistrarProximoContatoRequestContract request)
        {
            await _service.RegistrarProximoContato(id, request);
            return NoContent();
        }

        /// <summary> Registra um parecer e encerra o atendimento.</summary>
        [HttpPost("{id}/registrar-parecer-encerrar")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RegistrarParecerEEncerrar(long id, [FromBody] AtendimentoRegistrarParecerRequestContract request)
        {
            var usuarioLogadoId = ObterIdUsuarioLogado();
            await _service.RegistrarParecerEEncerrar(id, usuarioLogadoId, request);
            return Ok();
        }

        /// <summary> Registra um parecer e define próximo contato para o atendimento.</summary>
        [HttpPost("{id}/registrar-parecer-proximo-contato")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RegistrarParecerEProximoContato(long id, [FromBody] AtendimentoRegistrarParecerProximoContatoRequestContract request)
        {
            var usuarioLogadoId = ObterIdUsuarioLogado();
            await _service.RegistrarParecerEProximoContato(id, usuarioLogadoId, request);
            return Ok();
        }
    }
}
