using Microsoft.EntityFrameworkCore;
using UnCRM.Api.Data.Mappings;
using UnCRM.Api.Domain.Models;

namespace UnCRM.Api.Data
{
    public class ApplicationContext(DbContextOptions<ApplicationContext> options) : DbContext(options)
    {
        public DbSet<Usuario> Usuario { get; set; }
        public DbSet<Pessoa> Pessoa { get; set; }
        public DbSet<TipoAtendimento> TipoAtendimento { get; set; }
        public DbSet<Atendimento> Atendimento { get; set; }
        public DbSet<Parecer> Parecer { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UsuarioMap());
            modelBuilder.ApplyConfiguration(new PessoaMap());
            modelBuilder.ApplyConfiguration(new TipoAtendimentoMap());
            modelBuilder.ApplyConfiguration(new AtendimentoMap());
            modelBuilder.ApplyConfiguration(new ParecerMap());
        }
    }
}