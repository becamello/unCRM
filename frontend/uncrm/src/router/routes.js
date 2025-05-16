import HomeView from "@/views/HomeView.vue";
import LoginPage from "@/views/Login.vue";

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
];

export default routes;
