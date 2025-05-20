import tipoAtendimentoService from "@/services/tipoAtendimentoService";
import TipoAtendimento from "@/models/TipoAtendimento";

export async function carregarTodosTiposAgendamento() {
  try {
    const resultado = await tipoAtendimentoService.obterTodos();
    return resultado.data.map((u) => new TipoAtendimento(u));
  } catch (erro) {
    console.error("Erro ao carregar os tipos de atendimento:", erro);
  }
}