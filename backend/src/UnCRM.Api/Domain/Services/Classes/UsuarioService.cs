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
    public class UsuarioService : IUsuarioService
    {
          private readonly IUsuarioRepository _usuarioRepository;
        private readonly IMapper _mapper;

        private readonly TokenService _tokenService;

        public UsuarioService(
            IUsuarioRepository usuarioRepository,
            IMapper mapper,
            TokenService tokenService)
        {
            _usuarioRepository = usuarioRepository;
            _mapper = mapper;
            _tokenService = tokenService;
        }

        public async Task<UsuarioResponseContract> Adicionar(UsuarioRequestContract entidade, long idUsuario)
        {
            var usuario = _mapper.Map<Usuario>(entidade);

            usuario.Senha = GerarHashSenha(usuario.Senha);
            usuario.DataCadastro = DateTime.Now;

            usuario = await _usuarioRepository.Adicionar(usuario);

            return _mapper.Map<UsuarioResponseContract>(usuario);
        }

        public async Task<UsuarioResponseContract> Atualizar(long id, UsuarioRequestContract entidade, long idUsuario)
        {
            _ = await Obter(id) ?? throw new NotFoundException("Usuario não encontrado para atualização.");

            var usuario = _mapper.Map<Usuario>(entidade);
            usuario.Id = id;
            usuario.Senha = GerarHashSenha(entidade.Senha);

            usuario = await _usuarioRepository.Atualizar(usuario);

            return _mapper.Map<UsuarioResponseContract>(usuario);
        }

        public async Task<UsuarioLoginResponseContract> Autenticar(UsuarioLoginRequestContract usuarioLoginRequest)
        {
            var usuario = await _usuarioRepository.Obter(usuarioLoginRequest.Login);

            if (usuario is null)
                throw new AuthenticationException("Login não localizado");

            if (usuario.DataInativacao is not null)
                throw new AuthenticationException("O usuário encontra-se inativo");

            var hashSenha = GerarHashSenha(usuarioLoginRequest.Senha);

            if (usuario.Senha != hashSenha)
            {
                throw new AuthenticationException("Senha inválida.");
            }

            return new UsuarioLoginResponseContract
            {
                Id = usuario.Id,
                Login = usuario.Login,
                Token = _tokenService.GerarToken(usuario)
            };
        }

        public async Task Inativar(long id, long idUsuario)
        {
            var usuario = await _usuarioRepository.Obter(id) ?? throw new NotFoundException("Usuario não encontrado para inativação.");

            await _usuarioRepository.Deletar(_mapper.Map<Usuario>(usuario));
        }

        public async Task<UsuarioResponseContract> Obter(string login)
        {
            var usuario = await _usuarioRepository.Obter(login);
            return _mapper.Map<UsuarioResponseContract>(usuario);
        }

        public async Task<IEnumerable<UsuarioResponseContract>> Obter(long idUsuario)
        {
            var usuarios = await _usuarioRepository.Obter();

            return usuarios.Select(usuario => _mapper.Map<UsuarioResponseContract>(usuario));
        }

        public async Task<UsuarioResponseContract> Obter(long id, long idUsuario)
        {
            var usuario = await _usuarioRepository.Obter(id);

            if (usuario == null)
            {
                throw new NotFoundException("Usuário não encontrado.");
            }

            return _mapper.Map<UsuarioResponseContract>(usuario);
        }

        public string GerarHashSenha(string senha)
        {
            string hashSenha;

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytesSenha = Encoding.UTF8.GetBytes(senha);
                byte[] bytesHashSenha = sha256.ComputeHash(bytesSenha);
                hashSenha = BitConverter.ToString(bytesHashSenha).Replace("-", "").Replace("-", "").ToLower();
            }

            return hashSenha;
        }
    }
}