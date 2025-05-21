export default class Usuario {
  constructor(dados = {}) {
    this.id = dados.id;
    this.login = dados.login;
    this.nome = dados.nome;
    this.senha = dados.senha;
    this.dataInativacao = dados.dataInativacao;
    this.dataCadastro = dados.dataCadastro;
    this.token = dados.token;
    this.cargo = dados.cargo;
  }

  static cargoMap = {
    0: "Gerente",
    1: "Supervisor",
    2: "Atendente",
  };

  get cargoDescricao() {
    return Usuario.cargoMap[this.cargo] || "Desconhecido";
  }
  
  get isGerente() {
    return this.cargo === 0;
  }

  get dataCadastroFormatada() {
    return this.dataCadastro
      ? new Date(this.dataCadastro).toLocaleDateString("pt-BR")
      : "-";
  }

  get statusItem() {
    return this.dataInativacao ? "Inativo" : "Ativo";
  }
}
