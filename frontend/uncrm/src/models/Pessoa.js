export default class Pessoa {
  constructor(dados = {}) {
    this.id = dados.id;
    this.nome = dados.nome;
    this.nomeCurto = dados.nomeCurto;
    this.cpfCnpj = dados.cpfCnpj;
    this.tipoPessoa = dados.tipoPessoa;
    this.dataInativacao = dados.dataInativacao;
    this.dataCadastro = dados.dataCadastro;
  }

  static tipoPessoaMap = {
    0: "Física",
    1: "Jurídica",
  };

  get tipoPessoaDescricao() {
    return Pessoa.tipoPessoaMap[this.tipoPessoa];
  }

  get cpfCnpjFormatado() {
    const cnpjCpf = this.cpfCnpj?.replace(/\D/g, "");
    if (!cnpjCpf) return "-";
    if (cnpjCpf.length === 11) {
      return cnpjCpf.replace(/(\d{3})(\d{3})(\d{3})(\d{2})/, "$1.$2.$3-$4");
    }
    return cnpjCpf.replace(
      /(\d{2})(\d{3})(\d{3})(\d{4})(\d{2})/,
      "$1.$2.$3/$4-$5"
    );
  }

  get statusItem() {
    return this.dataInativacao ? "Inativo" : "Ativo";
  }

  get dataCadastroFormatada() {
    return this.dataCadastro
      ? new Date(this.dataCadastro).toLocaleDateString("pt-BR")
      : "-";
  }
}
