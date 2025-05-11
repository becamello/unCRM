using AutoMapper;
using UnCRM.Api.Contract.Pessoa;
using UnCRM.Api.Domain.Enums;
using UnCRM.Api.Domain.Models;
using UnCRM.Api.Domain.Repository.Interfaces;
using UnCRM.Api.Domain.Services.Interfaces;
using UnCRM.Api.Exceptions;

namespace UnCRM.Api.Domain.Services.Classes
{
    public class PessoaService(IPessoaRepository pessoaRepository, IMapper mapper) : IPessoaService
    {
        private readonly IPessoaRepository _pessoaRepository = pessoaRepository;
        private readonly IMapper _mapper = mapper;

        public async Task<PessoaResponseContract> Adicionar(PessoaRequestContract request)
        {
            if (string.IsNullOrWhiteSpace(request.Nome))
                throw new BadRequestException("Nome é obrigatório.");

            if (string.IsNullOrWhiteSpace(request.NomeCurto))
                throw new BadRequestException("Nome é obrigatório.");

            if (request.TipoPessoa == TipoPessoaEnum.PessoaFisica)
            {
                if (string.IsNullOrWhiteSpace(request.CpfCnpj))
                    throw new BadRequestException("CPF é obrigatório para pessoa física.");

                if (!ValidarCpf(request.CpfCnpj))
                    throw new BadRequestException("CPF inválido.");
            }
            else if (request.TipoPessoa == TipoPessoaEnum.PessoaJuridica)
            {
                if (string.IsNullOrWhiteSpace(request.CpfCnpj))
                    throw new BadRequestException("CNPJ é obrigatório para pessoa jurídica.");

                if (!ValidarCnpj(request.CpfCnpj))
                    throw new BadRequestException("CNPJ inválido.");
            }
            else
            {
                throw new BadRequestException("Tipo de pessoa inválido.");
            }

            var pessoa = _mapper.Map<Pessoa>(request);
            pessoa.DataCadastro = DateTime.Now;

            pessoa = await _pessoaRepository.Adicionar(pessoa);

            return _mapper.Map<PessoaResponseContract>(pessoa);
        }


        public async Task<PessoaResponseContract> Atualizar(long id, PessoaRequestContract request)
        {
            var entidade = await _pessoaRepository.Obter(id) ?? throw new NotFoundException("Pessoa não encontrada.");

            if (string.IsNullOrWhiteSpace(request.Nome))
                throw new BadRequestException("Nome é obrigatório.");

            if (string.IsNullOrWhiteSpace(request.NomeCurto))
                throw new BadRequestException("Nome é obrigatório.");

            if (request.TipoPessoa == TipoPessoaEnum.PessoaFisica)
            {
                if (string.IsNullOrWhiteSpace(request.CpfCnpj))
                    throw new BadRequestException("CPF é obrigatório para Pessoa Física.");

                if (!ValidarCpf(request.CpfCnpj))
                    throw new BadRequestException("CPF inválido.");
            }
            else if (request.TipoPessoa == TipoPessoaEnum.PessoaJuridica)
            {
                if (string.IsNullOrWhiteSpace(request.CpfCnpj))
                    throw new BadRequestException("CNPJ é obrigatório para Pessoa Jurídica.");

                if (!ValidarCnpj(request.CpfCnpj))
                    throw new BadRequestException("CNPJ inválido.");
            }
            else
            {
                throw new BadRequestException("Tipo de pessoa inválido.");
            }

            _mapper.Map(request, entidade);

            await _pessoaRepository.Atualizar(entidade);

            return _mapper.Map<PessoaResponseContract>(entidade);
        }


        public async Task Inativar(long id)
        {
            var pessoa = await _pessoaRepository.Obter(id) ?? throw new NotFoundException("Pessoa não encontrada para inativação.");

            await _pessoaRepository.Deletar(_mapper.Map<Pessoa>(pessoa));
        }

        public async Task<IEnumerable<PessoaResponseContract>> ObterTodos()
        {
            var pessoas = await _pessoaRepository.Obter();

            return pessoas.Select(_mapper.Map<PessoaResponseContract>);
        }

        public async Task<PessoaResponseContract> ObterPorId(long id)
        {
            var pessoa = await _pessoaRepository.Obter(id) ?? throw new NotFoundException("Pessoa não encontrada.");
            return _mapper.Map<PessoaResponseContract>(pessoa);
        }

        private static bool ValidarCpf(string cpf)
        {
            int[] multiplicador1 = [10, 9, 8, 7, 6, 5, 4, 3, 2];
            int[] multiplicador2 = [11, 10, 9, 8, 7, 6, 5, 4, 3, 2];

            cpf = cpf.Trim().Replace(".", "").Replace("-", "");
            if (cpf.Length != 11)
                return false;

            for (int j = 0; j < 10; j++)
                if (j.ToString().PadLeft(11, char.Parse(j.ToString())) == cpf)
                    return false;

            string tempCpf = cpf[..9];
            int soma = 0;

            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];

            int resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            string digito = resto.ToString();
            tempCpf += digito;
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito += resto.ToString();

            return cpf.EndsWith(digito);
        }
        private static bool ValidarCnpj(string cnpj)
        {
            int[] multiplicador1 = [5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];
            int[] multiplicador2 = [6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2];

            cnpj = cnpj.Trim().Replace(".", "").Replace("-", "").Replace("/", "");
            if (cnpj.Length != 14)
                return false;

            string tempCnpj = cnpj.Substring(0, 12);
            int soma = 0;

            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];

            int resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            string digito = resto.ToString();
            tempCnpj += digito;
            soma = 0;
            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];

            resto = soma % 11;
            if (resto < 2)
                resto = 0;
            else
                resto = 11 - resto;

            digito += resto.ToString();

            return cnpj.EndsWith(digito);
        }
    }
}