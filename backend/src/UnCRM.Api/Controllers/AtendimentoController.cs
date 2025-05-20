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

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Criar([FromBody] AtendimentoCriarRequestContract request)
        {
            var usuarioLogadoId = ObterIdUsuarioLogado();
            return Created("", await _service.Criar(request, usuarioLogadoId));
        }

        [HttpPost("/parecer/{id}")]
        [Authorize]
        public async Task<IActionResult> RegistrarParecer(long id, [FromBody] AtendimentoRegistrarParecerRequestContract request)
        {
            var usuarioLogadoId = ObterIdUsuarioLogado();
            return Created("", await _service.RegistrarParecer(id, usuarioLogadoId, request));
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ObterTodos()
        {
            return Ok(await _service.ObterTodos());
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<IActionResult> ObterPorId(long id)
        {
            return Ok(await _service.ObterPorId(id));
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Atualizar(long id, [FromBody] AtendimentoCriarRequestContract request)
        {
            var usuarioLogadoId = ObterIdUsuarioLogado();
            await _service.Atualizar(id, usuarioLogadoId, request);
            return NoContent();
        }

        [HttpPut("{id}/parecer/{parecerId}")]
        [Authorize]
        public async Task<IActionResult> EditarParecer(long id, long parecerId, [FromBody] AtendimentoEditarParecerRequestContract request)
        {
            var usuarioLogadoId = ObterIdUsuarioLogado();
            await _service.EditarParecer(id, parecerId, request, usuarioLogadoId);
            return NoContent();
        }

        [HttpPut("{id}/reabrir")]
        [Authorize]
        public async Task<IActionResult> Reabrir(long id)
        {
            var usuarioLogadoId = ObterIdUsuarioLogado();
            await _service.Reabrir(id, usuarioLogadoId);
            return NoContent();
        }

        [HttpPut("{id}/encerrar")]
        [Authorize]
        public async Task<IActionResult> Encerrar(long id)
        {
            await _service.Encerrar(id);
            return NoContent();
        }

        [HttpPut("{id}/proximo-contato")]
        [Authorize]
        public async Task<IActionResult> RegistrarProximoContato(long id, [FromBody] AtendimentoRegistrarProximoContatoRequestContract request)
        {
            await _service.RegistrarProximoContato(id, request);
            return NoContent();
        }

        [HttpPost("{id}/registrar-parecer-encerrar")]
        public async Task<IActionResult> RegistrarParecerEEncerrar(long id, [FromBody] AtendimentoRegistrarParecerRequestContract request)
        {
            var usuarioLogadoId = ObterIdUsuarioLogado();
            await _service.RegistrarParecerEEncerrar(id, usuarioLogadoId, request);
            return Ok();
        }

        [HttpPost("{id}/registrar-parecer-proximo-contato")]
        public async Task<IActionResult> RegistrarParecerEProximoContato(long id, [FromBody] AtendimentoRegistrarParecerProximoContatoRequestContract request)
        {
            var usuarioLogadoId = ObterIdUsuarioLogado();
            await _service.RegistrarParecerEProximoContato(id, usuarioLogadoId, request);
            return Ok();
        }
    }
}
