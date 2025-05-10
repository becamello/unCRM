using AutoMapper;
using UnCRM.Api.Contract.Pessoa;
using UnCRM.Api.Domain.Enums;
using UnCRM.Api.Domain.Models;
using UnCRM.Api.Domain.Repository.Interfaces;
using UnCRM.Api.Domain.Services.Interfaces;
using UnCRM.Api.Exceptions;

namespace UnCRM.Api.Domain.Services.Classes
{
    public class PessoaService : IPessoaService
    {
        private readonly IPessoaRepository _pessoaRepository;
        private readonly IMapper _mapper;
        public PessoaService(IPessoaRepository pessoaRepository, IMapper mapper)
        {
            _pessoaRepository = pessoaRepository;
            _mapper = mapper;
        }
        public async Task<PessoaResponseContract> Adicionar(PessoaRequestContract entidade, long idPessoa)
        {
            if (entidade.TipoPessoa == TipoPessoaEnum.PF)
            {
                if (string.IsNullOrWhiteSpace(entidade.Cpf))
                    throw new BadRequestException("CPF é obrigatório para pessoa física.");

                if (!ValidarCpf(entidade.Cpf))
                    throw new BadRequestException("CPF inválido.");

                entidade.Cnpj = string.Empty; 
            }
            else if (entidade.TipoPessoa == TipoPessoaEnum.PJ)
            {
                if (string.IsNullOrWhiteSpace(entidade.Cnpj))
                    throw new BadRequestException("CNPJ é obrigatório para pessoa jurídica.");

                if (!ValidarCnpj(entidade.Cnpj))
                    throw new BadRequestException("CNPJ inválido.");

                entidade.Cpf = string.Empty; 
            }

            var pessoa = _mapper.Map<Pessoa>(entidade);
            pessoa.DataCadastro = DateTime.Now;

            pessoa = await _pessoaRepository.Adicionar(pessoa);

            return _mapper.Map<PessoaResponseContract>(pessoa);
        }

        public async Task<PessoaResponseContract> Atualizar(long id, PessoaRequestContract entidade, long idPessoa)
        {
            _ = await Obter(id) ?? throw new NotFoundException("Pessoa não encontrada para atualização.");

            if (entidade.TipoPessoa == TipoPessoaEnum.PF)
            {
                if (string.IsNullOrEmpty(entidade.Cpf))
                {
                    throw new ArgumentException("CPF é obrigatório para Pessoa Física.");
                }
                if (!ValidarCpf(entidade.Cpf))
                {
                    throw new ArgumentException("CPF inválido.");
                }
                entidade.Cnpj = string.Empty;
            }
            else if (entidade.TipoPessoa == TipoPessoaEnum.PJ)
            {
                if (string.IsNullOrEmpty(entidade.Cnpj))
                {
                    throw new ArgumentException("CNPJ é obrigatório para Pessoa Jurídica.");
                }

                if (!ValidarCnpj(entidade.Cnpj))
                {
                    throw new ArgumentException("CNPJ inválido.");
                }
                entidade.Cpf = string.Empty;
            }

            var pessoa = _mapper.Map<Pessoa>(entidade);
            pessoa.Id = id;

            pessoa = await _pessoaRepository.Atualizar(pessoa);

            return _mapper.Map<PessoaResponseContract>(pessoa);
        }

        public async Task Inativar(long id, long idPessoa)
        {
            var pessoa = await _pessoaRepository.Obter(id) ?? throw new NotFoundException("Pessoa não encontrada para inativação.");

            await _pessoaRepository.Deletar(_mapper.Map<Pessoa>(pessoa));
        }

        public async Task<IEnumerable<PessoaResponseContract>> Obter(long idPessoa)
        {
            var pessoas = await _pessoaRepository.Obter();

            return pessoas.Select(pessoa => _mapper.Map<PessoaResponseContract>(pessoa));
        }

        public async Task<PessoaResponseContract> Obter(long id, long idPessoa)
        {
            var pessoa = await _pessoaRepository.Obter(id);

            if (pessoa == null)
            {
                throw new NotFoundException("Pessoa não encontrada.");
            }

            return _mapper.Map<PessoaResponseContract>(pessoa);
        }

        private bool ValidarCpf(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf) || cpf.Length != 11)
                return false;
            if (!cpf.All(char.IsDigit))
            {
                return false;
            }

            return true;
        }
        private bool ValidarCnpj(string cnpj)
        {
            if (string.IsNullOrWhiteSpace(cnpj) || cnpj.Length != 14)
                return false;
            if (!cnpj.All(char.IsDigit))
            {
                return false;
            }

            return true;
        }
    }
}