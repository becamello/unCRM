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

function obterUsuarioIdDoToken() {
  let token = obterTokenNaStorage();
  if (!token) return null;

  try {
    let payloadBase64 = token.split(".")[1];
    let payload = JSON.parse(atob(payloadBase64));
    return payload.nameid || payload.id || null;
  } catch (e) {
    console.error("Erro ao decodificar token:", e);
    return null;
  }
}

export default {
  salvarStorage,
  obterStorage,
  removerStorage,
  salvarTokenNaStorage,
  obterTokenNaStorage,
  removerTokenNaStorage,
  obterUsuarioIdDoToken
};
