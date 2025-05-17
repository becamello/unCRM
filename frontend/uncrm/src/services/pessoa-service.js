import api from "./api";

function cadastrar(pessoa) {
  return new Promise((resolve, reject) => {
    return api
      .post(`/pessoas`, pessoa)
      .then((response) => resolve(response))
      .catch((error) => reject(error));
  });
}
function obterTodos() {
  return new Promise((resolve, reject) => {
    return api
      .get("/pessoas")
      .then((response) => resolve(response))
      .catch((error) => reject(error));
  });
}
function obterPorId(id) {
  return new Promise((resolve, reject) => {
    return api
      .get(`/pessoas/${id}`)

      .then((response) => resolve(response))
      .catch((error) => reject(error));
  });
}
function atualizar(pessoa) {
  return new Promise((resolve, reject) => {
    return api
      .put(`/pessoas/${pessoa.id}`, pessoa)
      .then((response) => resolve(response))
      .catch((error) => reject(error));
  });
}
function inativar(pessoa) {
  return new Promise((resolve, reject) => {
    return api
      .delete(`/pessoas/${pessoa.id}`, pessoa)
      .then((response) => resolve(response))
      .catch((error) => reject(error));
  });
}

export default {
  obterTodos,
  obterPorId,
  cadastrar,
  atualizar,
  inativar,
};
