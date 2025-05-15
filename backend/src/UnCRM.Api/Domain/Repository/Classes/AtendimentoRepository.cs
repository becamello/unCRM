using Microsoft.EntityFrameworkCore;
using UnCRM.Api.Data;
using UnCRM.Api.Domain.Models;
using UnCRM.Api.Domain.Repository.Interfaces;

namespace UnCRM.Api.Domain.Repository.Classes
{
    public class AtendimentoRepository(ApplicationContext context) : Repository<Atendimento, long>(context), IAtendimentoRepository
    {
        public override async Task<Atendimento> Obter(long id)
        {
            return await context
            .Atendimento
            .Include(x => x.Pareceres)
            .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
