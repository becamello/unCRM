import CRMAtendimentos from "@/views/CRM/Atendimentos.vue";
import HistoricoAtendimento from "@/views/CRM/HistoricoAtendimento.vue";
import HomeView from "@/views/HomeView.vue";
import LoginPage from "@/views/Login.vue";
import InicialPessoas from "@/views/Pessoas/InicialPessoas.vue";
import InicialUsuarios from "@/views/Usuarios/InicialUsuarios.vue";

const routes = [
  {
    path: "*",
    redirect: "/login",
  },
  {
    path: "/login",
    name: "Login",
    component: LoginPage,
    meta: {
      title: "Login",
      requiredAuth: false,
    },
  },
  {
    path: "/",
    name: "Inicial",
    component: HomeView,
    meta: {
      title: "Página Inicial",
      requiredAuth: true,
    },
  },
  {
    path: "/crm-atendimentos",
    name: "Atendimentos",
    component: CRMAtendimentos,
    meta: {
      title: "Atendimentos",
      requiredAuth: true,
    },
  },
  {
    path: "/historico-atendimento/:id",
    name: "HistoricoAtendimento",
    component: HistoricoAtendimento,
    meta: {
      title: "Histórico atendimento",
      requiredAuth: true,
    },
  },
  {
    path: "/usuarios",
    name: "Usuários",
    component: InicialUsuarios,
    meta: {
      title: "Usuários",
      requiredAuth: true,
    },
  },
  {
    path: "/pessoas",
    name: "Pessoas",
    component: InicialPessoas,
    meta: {
      title: "Pessoas",
      requiredAuth: true,
    },
  },
];

export default routes;
