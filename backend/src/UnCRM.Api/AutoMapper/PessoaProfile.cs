using AutoMapper;
using UnCRM.Api.Contract.Pessoa;
using UnCRM.Api.Domain.Models;

namespace UnCRM.Api.AutoMapper
{
    public class PessoaProfile : Profile
    {
        public PessoaProfile()
        {
            CreateMap<Pessoa, PessoaRequestContract>().ReverseMap();
            CreateMap<Pessoa, PessoaResponseContract>().ReverseMap();
            
        }
    }
}