import usuarioService from "@/services/usuario-service";
import Usuario from "@/models/Usuario";

export async function carregarTodosUsuarios() {
  try {
    const resultado = await usuarioService.obterTodos();
    return resultado.data.map((u) => new Usuario(u));
  } catch (erro) {
    console.error("Erro ao carregar usuários:", erro);
  }
}