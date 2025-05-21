import Vue from "vue";
import VueRouter from "vue-router";
import routes from "./routes"
import utilStorage from "@/utils/storage";
import Usuario from "@/models/Usuario";

Vue.use(VueRouter);

const router = new VueRouter({
  mode: "history",
  base: process.env.BASE_URL,
  routes,
});

router.afterEach((to) => {
  const pageTitle = to.meta.title;
  document.title = `${pageTitle} - UnCRM`;
});

router.beforeEach((to, from, next) => {
  const token = localStorage.getItem("token");

  if (!token && to.meta.requiredAuth) {
    return next({ name: "Login" });
  }

  if (token && to.meta.requiredAuth) {
    const usuario = utilStorage.obterCargoNaStorage();
    const user = new Usuario(usuario);

    const rota = to.path;

    const permissoes = {
      Gerente: ["/", "/crm-atendimentos", "/usuarios", "/pessoas"],
      Supervisor: ["/", "/crm-atendimentos", "/usuarios"],
      Atendente: ["/", "/crm-atendimentos"],
    };

    const rotasPermitidas = permissoes[user.role] || [];

    if (!rotasPermitidas.includes(rota)) {
      return next({ name: "StatusCodePage403" });
    }
  }

  next();
});

export default router;
