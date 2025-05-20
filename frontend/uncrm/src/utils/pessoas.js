import pessoaService from "@/services/pessoaService";
import Pessoa from "@/models/Pessoa";

export async function carregarTodasPessoas() {
  try {
    const resultado = await pessoaService.obterTodos();
    return resultado.data.map((u) => new Pessoa(u));
  } catch (erro) {
    console.error("Erro ao carregar pessoas:", erro);
  }
}

export async function carregarTodasPessoasAtivas() {
  try {
    const resultado = await pessoaService.obterTodos();

    return resultado.data
      .map((p) => new Pessoa(p))
      .filter((pessoa) => !pessoa.dataInativacao); 
  } catch (erro) {
    console.error("Erro ao carregar pessoas:", erro);
    return [];
  }
}
