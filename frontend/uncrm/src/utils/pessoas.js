import pessoaService from "@/services/pessoa-service";
import Pessoa from "@/models/Pessoa";

export async function carregarTodasPessoas() {
  try {
    const resultado = await pessoaService.obterTodos();
    return resultado.data.map((u) => new Pessoa(u));
  } catch (erro) {
    console.error("Erro ao carregar pessoas:", erro);
  }
}