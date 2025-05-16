import Usuario from "@/models/Usuario";

function salvarStorage(usuario) {
  let usuarioString = JSON.stringify(usuario);
  localStorage.setItem("usuario", usuarioString);
}

function obterStorage() {
  let usuarioString = localStorage.getItem("usuario");
  return new Usuario(JSON.parse(usuarioString));
}

function removerStorage() {
  localStorage.removeItem("usuario");
}

function salvarTokenNaStorage(token) {
  localStorage.setItem("token", token);
}

function obterTokenNaStorage() {
  return localStorage.getItem("token");
}
function removerTokenNaStorage() {
  localStorage.removeItem("token");
}

export default {
  salvarStorage,
  obterStorage,
  removerStorage,
  salvarTokenNaStorage,
  obterTokenNaStorage,
  removerTokenNaStorage,
};
