import api from "./api";

function login(login, senha) {
  return new Promise((resolve, reject) => {
    return api
      .post(`/usuarios/login`, { login, senha })
      .then((response) => resolve(response))
      .catch((error) => reject(error));
  });
}

function cadastrar(usuario) {
  return new Promise((resolve, reject) => {
    return api
      .post(`/usuarios`, usuario)
      .then((response) => resolve(response))
      .catch((error) => reject(error));
  });
}
function obterTodos() {
  return new Promise((resolve, reject) => {
    return api
      .get("/usuarios")
      .then((response) => resolve(response))
      .catch((error) => reject(error));
  });
}
function obterPorId(id) {
  return new Promise((resolve, reject) => {
    return api
      .get(`/usuarios/${id}`)

      .then((response) => resolve(response))
      .catch((error) => reject(error));
  });
}
function atualizar(usuario) {
  return new Promise((resolve, reject) => {
    return api
      .put(`/usuarios/${usuario.id}`, usuario)
      .then((response) => resolve(response))
      .catch((error) => reject(error));
  });
}
function inativar(usuario) {
  return new Promise((resolve, reject) => {
    return api
      .delete(`/usuarios/${usuario.id}`, usuario)
      .then((response) => resolve(response))
      .catch((error) => reject(error));
  });
}

export default {
  login,
  obterTodos,
  obterPorId,
  cadastrar,
  atualizar,
  inativar,
};
