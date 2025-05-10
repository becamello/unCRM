namespace UnCRM.Api.Contract.Pessoa
{
    public class PessoaResponseContract : PessoaRequestContract
    {
        public long Id { get; set; }
        public DateTime DataCadastro { get; set; }
    }
}