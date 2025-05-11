using System.Security.Authentication;
using System.Security.Cryptography;
using System.Text;
using AutoMapper;
using UnCRM.Api.Contract.Usuario;
using UnCRM.Api.Domain.Models;
using UnCRM.Api.Domain.Repository.Interfaces;
using UnCRM.Api.Domain.Services.Interfaces;
using UnCRM.Api.Exceptions;

namespace UnCRM.Api.Domain.Services.Classes
{
    public class UsuarioService(
        IUsuarioRepository usuarioRepository,
        IMapper mapper,
        TokenService tokenService) : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository = usuarioRepository;
        private readonly IMapper _mapper = mapper;
        private readonly TokenService _tokenService = tokenService;
        public async Task<UsuarioResponseContract> Adicionar(UsuarioRequestContract request)
        {
            if (string.IsNullOrWhiteSpace(request.Nome))
                throw new BadRequestException("Nome é obrigatório.");

            if (string.IsNullOrWhiteSpace(request.Login))
                throw new BadRequestException("Login é obrigatório.");

            if (string.IsNullOrWhiteSpace(request.Senha))
                throw new BadRequestException("Senha é obrigatória.");

            var usuario = _mapper.Map<Usuario>(request);
            usuario.Senha = GerarHashSenha(usuario.Senha);
            usuario.DataCadastro = DateTime.Now;

            usuario = await _usuarioRepository.Adicionar(usuario);

            return _mapper.Map<UsuarioResponseContract>(usuario);
        }
        public async Task<UsuarioResponseContract> Atualizar(long id, UsuarioRequestContract request)
        {
            var entidade = await _usuarioRepository.Obter(id) ?? throw new NotFoundException("Usuário não encontrado para atualização.");

            if (string.IsNullOrWhiteSpace(request.Nome))
                throw new BadRequestException("Nome é obrigatório.");

            if (string.IsNullOrWhiteSpace(request.Login))
                throw new BadRequestException("Login é obrigatório.");

            if (string.IsNullOrWhiteSpace(request.Senha))
                throw new BadRequestException("Senha é obrigatória.");

            _mapper.Map(request, entidade);

            entidade.Id = id;
            entidade.Senha = GerarHashSenha(request.Senha);

            await _usuarioRepository.Atualizar(entidade);

            return _mapper.Map<UsuarioResponseContract>(entidade);
        }

        public async Task<UsuarioLoginResponseContract> Autenticar(UsuarioLoginRequestContract request)
        {
            var usuario = await _usuarioRepository.Obter(request.Login);

            if (usuario == null)
                throw new AuthenticationException("Login não localizado.");

            if (usuario.DataInativacao != null)
                throw new AuthenticationException("O usuário encontra-se inativo.");

            var hashSenha = GerarHashSenha(request.Senha);

            if (usuario.Senha != hashSenha)
                throw new AuthenticationException("Senha inválida.");

            return new UsuarioLoginResponseContract
            {
                Id = usuario.Id,
                Login = usuario.Login,
                Token = _tokenService.GerarToken(usuario)
            };
        }

        public async Task Inativar(long id)
        {
            var usuario = await _usuarioRepository.Obter(id) ?? throw new NotFoundException("Usuário não encontrado para inativação.");

            await _usuarioRepository.Deletar(usuario);
        }

        public async Task<UsuarioResponseContract> ObterPorLogin(string login)
        {
            var usuario = await _usuarioRepository.Obter(login)
                           ?? throw new NotFoundException("Usuário não encontrado.");

            return _mapper.Map<UsuarioResponseContract>(usuario);
        }

        private static string GerarHashSenha(string senha)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(senha);
            var hash = sha256.ComputeHash(bytes);
            return BitConverter.ToString(hash).Replace("-", "").ToLower();
        }

        public async Task<IEnumerable<UsuarioResponseContract>> ObterTodos()
        {
            var usuarios = await _usuarioRepository.Obter();

            return usuarios.Select(_mapper.Map<UsuarioResponseContract>);
        }

        public async Task<UsuarioResponseContract> ObterPorId(long id)
        {
            var usuario = await _usuarioRepository.Obter(id)
                           ?? throw new NotFoundException("Usuário não encontrado.");

            return _mapper.Map<UsuarioResponseContract>(usuario);
        }
    }
}