import api from "./api";

function obterTodos() {
  return new Promise((resolve, reject) => {
    return api
      .get("/tipo-atendimento")
      .then((response) => resolve(response))
      .catch((error) => reject(error));
  });
}

export default {
  obterTodos,

};
