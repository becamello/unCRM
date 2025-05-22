import CRMAtendimentos from "@/views/CRM/Atendimentos.vue";
import HistoricoAtendimento from "@/views/CRM/HistoricoAtendimento.vue";
import LoginPage from "@/views/Login.vue";
import InicialPessoas from "@/views/Pessoas/InicialPessoas.vue";
import InicialUsuarios from "@/views/Usuarios/InicialUsuarios.vue";
import StatusCodePage403 from "@/views/StatusCode/StatusCodePage403.vue";
import StatusCodePage404 from "@/views/StatusCode/StatusCodePage404.vue";
import DashboardPage from "@/views/DashboardPage.vue";
import TelaInicial from "@/views/TelaInicial.vue";

const routes = [
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
    component: TelaInicial,
    meta: {
      title: "Página Inicial",
      requiredAuth: true,
    },
  },
    {
    path: "/403",
    name: "StatusCodePage403",
    component: StatusCodePage403,
    meta: {
      title: "Acesso Negado",
      requiredAuth: false,
    },
  },
  {
    path: "*",
    name: "StatusCodePage404",
    component: StatusCodePage404,
    meta: {
      title: "Página Não Encontrada",
      requiredAuth: false,
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
      rolesPermitidos: ["Gerente", "Supervisor"],
    },
  },
  {
    path: "/pessoas",
    name: "Pessoas",
    component: InicialPessoas,
    meta: {
      title: "Pessoas",
      requiredAuth: true,
      rolesPermitidos: ["Gerente"],
    },
  },
  {
    path: "/dashboard",
    name: "Dashboard",
    component: DashboardPage,
    meta: {
      title: "Dashboard",
      requiredAuth: true,
    },
  },
];

export default routes;
