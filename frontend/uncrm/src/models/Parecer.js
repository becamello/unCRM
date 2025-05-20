export default class Parecer {
  constructor(dados = {}) {
    this.id = dados.id;
    this.descricao = dados.descricao;
    this.usuarioCriadorId = dados.usuarioCriadorId ?? dados.usuarioCriacaoId;
    this.usuarioCriadorLogin = dados.usuarioCriadorLogin;
    this.data = dados.data;
    this.parecer = dados.parecer;
  }
}
