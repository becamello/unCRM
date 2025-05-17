import api from "./api";
import Atendimento from "@/models/Atendimento";
import Parecer from "@/models/Parecer";

function criar(atendimento, usuarioLogadoId) {
  return new Promise((resolve, reject) => {
    api
      .post(`/atendimentos?usuarioLogadoId=${usuarioLogadoId}`, atendimento)
      .then((response) => resolve(response.data))
      .catch((error) => reject(error));
  });
}

function registrarParecer(atendimentoId, usuarioLogadoId, request) {
  return new Promise((resolve, reject) => {
    api
      .post(`/atendimentos/${atendimentoId}/parecer?usuarioLogadoId=${usuarioLogadoId}`, request)
      .then((response) => resolve(new Parecer(response.data)))
      .catch((error) => reject(error));
  });
}

function obterTodos() {
  return new Promise((resolve, reject) => {
    api
      .get("/atendimento")
      .then((response) => {
        const lista = response.data.map((item) => new Atendimento(item));
        resolve(lista);
      })
      .catch((error) => reject(error));
  });
}

function obterPorId(id) {
  return new Promise((resolve, reject) => {
    api
      .get(`/atendimentos/${id}`)
      .then((response) => resolve(new Atendimento(response.data)))
      .catch((error) => reject(error));
  });
}

function atualizar(atendimento) {
  return new Promise((resolve, reject) => {
    api
      .put(`/atendimentos/${atendimento.id}`, atendimento)
      .then((response) => resolve(response.data))
      .catch((error) => reject(error));
  });
}

function editarParecer(atendimentoId, parecerId, request, usuarioLogadoId) {
  return new Promise((resolve, reject) => {
    api
      .put(`/atendimentos/${atendimentoId}/parecer/${parecerId}?usuarioLogadoId=${usuarioLogadoId}`, request)
      .then((response) => resolve(new Parecer(response.data)))
      .catch((error) => reject(error));
  });
}

function reabrir(id, usuarioLogadoId) {
  return new Promise((resolve, reject) => {
    api
      .post(`/atendimentos/${id}/reabrir?usuarioLogadoId=${usuarioLogadoId}`)
      .then((response) => resolve(response.data))
      .catch((error) => reject(error));
  });
}

function encerrar(id) {
  return new Promise((resolve, reject) => {
    api
      .post(`/atendimentos/${id}/encerrar`)
      .then((response) => resolve(response.data))
      .catch((error) => reject(error));
  });
}

export default {
  criar,
  registrarParecer,
  obterTodos,
  obterPorId,
  atualizar,
  editarParecer,
  reabrir,
  encerrar,
};
