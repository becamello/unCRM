export default class Usuario {
  constructor(dados = {}) {
    this.id = dados.id;
    this.login = dados.login;
    this.senha = dados.senha;
    this.dataInativacao = dados.dataInativacao;
    this.token = dados.token;
    this.cargo = dados.cargo;
  }

  static cargoMap = {
    0: "Gerente",
    1: "Supervisor",
    2: "Atendente"
  };

  get cargoDescricao() {
    return Usuario.cargoMap[this.cargo] || "Desconhecido";
  }
}
