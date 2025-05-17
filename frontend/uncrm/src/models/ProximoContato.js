export default class ProximoContato {
  constructor(dados = {}) {
    this.usuarioLogin = dados.usuarioLogin;
    this.data = dados.data;
  }

    get descricao() {
    const dataFormatada = new Date(this.data).toLocaleDateString("pt-BR");
    return `${dataFormatada} - ${this.usuarioLogin}`;
  }
}
