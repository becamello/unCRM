using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UnCRM.Api.Contract.Pessoa;
using UnCRM.Api.Domain.Services.Interfaces;
using UnCRM.Api.Exceptions;

namespace UnCRM.Api.Controllers
{
    [ApiController]
    [Route("pessoas")]
    public class PessoaController : BaseController
    {
        private readonly IService<PessoaRequestContract, PessoaResponseContract, long> _pessoaService;

        public PessoaController(IService<PessoaRequestContract, PessoaResponseContract, long> pessoaService)
        {
            _pessoaService = pessoaService;
        }

        // <summary> Adiciona uma nova pessoa </summary>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(PessoaResponseContract), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Adicionar(PessoaRequestContract contrato)
        {
            try
            {

                return Created("", await _pessoaService.Adicionar(contrato));
            }
            catch (NotFoundException ex)
            {
                return NotFound(RetornarModelNotFound(ex));
            }
            catch (BadRequestException ex)
            {
                return BadRequest(RetornarModelBadRequest(ex));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        // <summary> Obtém todas as pessoas. </summary>
        [HttpGet]
        [Authorize]
        [ProducesResponseType(typeof(IEnumerable<PessoaResponseContract>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Obter()
        {
            try
            {
                return Ok(await _pessoaService.ObterTodos());
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        // <summary> Obtém uma pessoa pelo ID. </summary>
        [HttpGet("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(PessoaResponseContract), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Obter(long id)
        {
            try
            {

                return Ok(await _pessoaService.ObterPorId(id));
            }
            catch (NotFoundException ex)
            {
                return NotFound(RetornarModelNotFound(ex));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        // <summary> Atualiza os dados de uma pessoa. </summary>
        [HttpPut("{id}")]
        [Authorize]
        [ProducesResponseType(typeof(PessoaResponseContract), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Atualizar(long id, PessoaRequestContract contrato)
        {
            try
            {
                return Ok(await _pessoaService.Atualizar(id, contrato));
            }

            catch (NotFoundException ex)
            {
                return NotFound(RetornarModelNotFound(ex));
            }
            catch (BadRequestException ex)
            {
                return BadRequest(RetornarModelBadRequest(ex));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        // <summary> Inativa uma pessoa. </summary>
        [HttpDelete("{id}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Deletar(long id)
        {
            try
            {
                await _pessoaService.Inativar(id);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(RetornarModelNotFound(ex));
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}