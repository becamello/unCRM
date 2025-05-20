export default class ProximoContato {
  constructor(dados = {}) {
    this.usuario = dados.usuario || null;
    this.usuarioLogin = dados.usuarioLogin || null;
    this.data = dados.data || null;
  }

    get descricao() {
    const dataFormatada = new Date(this.data).toLocaleDateString("pt-BR");
    return `${dataFormatada} - ${this.usuarioLogin}`;
  }

    get dataFormatada() {
    return this.data
      ? new Date(this.data).toLocaleDateString("pt-BR")
      : "-";
  }
}
