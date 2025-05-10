using AutoMapper;
using UnCRM.Api.Contract.Usuario;
using UnCRM.Api.Domain.Models;

namespace UnCRM.Api.AutoMapper
{
    public class UsuarioProfile : Profile
    {
         public UsuarioProfile()
        {
            CreateMap<Usuario, UsuarioRequestContract>().ReverseMap();
            CreateMap<Usuario, UsuarioResponseContract>().ReverseMap();
        }
    }
}