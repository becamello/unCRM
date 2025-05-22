 import atendimentoService from "@/services/atendimentoService";
 import Atendimento from "@/models/Atendimento";
 
export async function carregarAtendimentos(filtro = {}) {
  try {
    const resultado = await atendimentoService.obterTodos(filtro);
    return resultado.map((a) => new Atendimento(a));
  } catch (erro) {
    console.error("Erro ao carregar atendimentos:", erro);
    throw erro;
  }
}