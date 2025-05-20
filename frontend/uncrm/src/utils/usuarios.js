import usuarioService from "@/services/usuarioService";
import Usuario from "@/models/Usuario";

export async function carregarTodosUsuarios() {
  try {
    const resultado = await usuarioService.obterTodos();
    return resultado.data.map((u) => new Usuario(u));
  } catch (erro) {
    console.error("Erro ao carregar usuários:", erro);
  }
}

export async function carregarTodosUsuariosAtivos() {
  try {
    const resultado = await usuarioService.obterTodos();

    return resultado.data
      .map((p) => new Usuario(p))
      .filter((usuario) => !usuario.dataInativacao); 
  } catch (erro) {
    console.error("Erro ao carregar usuarios:", erro);
    return [];
  }
}
