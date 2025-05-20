import ProximoContato from "./ProximoContato";

export default class Atendimento {
  constructor(dados = {}) {
    this.id = dados.id;
    this.tipoAtendimentoDescricao = dados.tipoAtendimentoDescricao;
    this.tipoAtendimentoId = dados.tipoAtendimentoId ?? null;
    this.pessoaNome = dados.pessoaNome;
    this.pessoaId = dados.pessoaId ?? null;
    this.usuarioCriadorLogin = dados.usuarioCriadorLogin;
    this.status = dados.statusAtendimento ?? dados.status;
    this.dataCadastro = dados.dataCadastro;
    this.dataInativacao = dados.dataInativacao;
    this.proximoContato = dados.proximoContato
    ? new ProximoContato(dados.proximoContato)
    : new ProximoContato();
    this.parecer = dados.parecer;
    this.pareceres = dados.pareceres;
  }

  static statusMap = {
    0: "Aberto",
    1: "Encerrado",
  };

  get statusItem() {
    return Atendimento.statusMap[this.status] || "Desconhecido";
  }

  get dataCadastroFormatada() {
    return this.dataCadastro
      ? new Date(this.dataCadastro).toLocaleDateString("pt-BR")
      : "-";
  }

  get dataUsuarioCadastro() {
    return `${this.dataCadastroFormatada} - ${this.usuarioCriadorLogin ?? "-"}`;
  }

  get idFormatado() {
    return this.id.toString().padStart(6, '0') ?? " ";
  }
}
