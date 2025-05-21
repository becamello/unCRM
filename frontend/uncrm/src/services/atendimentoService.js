import api from "./api";
import Atendimento from "@/models/Atendimento";
import Parecer from "@/models/Parecer";

function criar(atendimento, usuarioLogadoId) {
  return new Promise((resolve, reject) => {
    api
      .post(`/atendimento?usuarioLogadoId=${usuarioLogadoId}`, atendimento)
      .then((response) => resolve(response.data))
      .catch((error) => reject(error));
  });
}

function registrarParecer(atendimentoId, request) {
  return new Promise((resolve, reject) => {
    api
      .post(`/parecer/${atendimentoId}`, request)
      .then((response) => resolve(new Parecer(response.data)))
      .catch((error) => reject(error));
  });
}

function obterTodos(filtro) {
  const filtroFormatado = formatarFiltro(filtro);
  return new Promise((resolve, reject) => {
    api
      .get(`/atendimento?${filtroFormatado}`)
      .then((response) => {
        const lista = response.data.map((item) => new Atendimento(item));
        resolve(lista);
      })
      .catch((error) => reject(error));
  });
}

function formatarFiltro(params) {
  const query = Object.entries(params)
    .flatMap(([key, value]) => {
      if (value == null) return [];
      if (Array.isArray(value)) {
        if (value.length === 0) return []; 
        return value.map(v => `${encodeURIComponent(key)}=${encodeURIComponent(v)}`);
      }
      return [`${encodeURIComponent(key)}=${encodeURIComponent(value)}`];
    })
    .join("&");

  return query ? `${query}` : "";
}

function obterPorId(id) {
  return new Promise((resolve, reject) => {
    api
      .get(`/atendimento/${id}`)
      .then((response) => resolve(response))
      .catch((error) => reject(error));
  });
}

function atualizar(atendimento) {
  return new Promise((resolve, reject) => {
    api
      .put(`/atendimento/${atendimento.id}`, atendimento)
      .then((response) => resolve(response.data))
      .catch((error) => reject(error));
  });
}

function editarParecer(atendimentoId, parecerId, request) {
  return new Promise((resolve, reject) => {
    api
      .put(`/atendimento/${atendimentoId}/parecer/${parecerId}`, request)
      .then((response) => resolve(new Parecer(response.data)))
      .catch((error) => reject(error));
  });
}

function reabrir(id) {
  return new Promise((resolve, reject) => {
    api
      .put(`/atendimento/${id}/reabrir`)
      .then((response) => resolve(response.data))
      .catch((error) => reject(error));
  });
}

function encerrar(id) {
  return new Promise((resolve, reject) => {
    api
      .put(`/atendimento/${id}/encerrar`)
      .then((response) => resolve(response.data))
      .catch((error) => reject(error));
  });
}

function registrarProximoContato(id, request) {
  return new Promise((resolve, reject) => {
    api
      .put(`/atendimento/${id}/proximo-contato`, request)
      .then((response) => resolve(response.data))
      .catch((error) => reject(error));
  });
}

function registrarParecerComProximoContato(id, request) {
  return new Promise((resolve, reject) => {
    api
      .post(`/atendimento/${id}/registrar-parecer-proximo-contato`, request)
      .then((response) => resolve(response.data))
      .catch((error) => reject(error));
  });
}

function registrarParecerComEncerramento(id, request) {
  return new Promise((resolve, reject) => {
    api
      .post(`/atendimento/${id}/registrar-parecer-encerrar`, request)
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
  registrarProximoContato,
  registrarParecerComProximoContato,
  registrarParecerComEncerramento,
  formatarFiltro
};
