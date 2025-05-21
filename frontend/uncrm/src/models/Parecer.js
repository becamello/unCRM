export default class Parecer {
  constructor(dados = {}) {
    this.id = dados.id;
    this.descricao = dados.descricao;
    this.usuarioCriadorId = dados.usuarioCriadorId ?? dados.usuarioCriacaoId;
    this.usuarioCriadorLogin = dados.usuarioCriadorLogin;
    this.data = dados.data;
    this.parecer = dados.parecer;
  }

  get dataFormatada() {
    if (!this.data) return "-";

    const data = new Date(this.data);
    const dataStr = data.toLocaleDateString("pt-BR");
    const horaStr = data.toLocaleTimeString("pt-BR", {
      hour: "2-digit",
      minute: "2-digit",
      hour12: false,
    });

    return `${dataStr} - ${horaStr}`;
  }
}
