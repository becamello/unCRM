import Vue from "vue";
import VueRouter from "vue-router";
import routes from "./routes"

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

  if (to.name === "Login") {
    if (token) {
      next({ name: "Inicial" });
    } else {
      next();
    }
  } else if (to.meta.requiredAuth) {
    if (!token || token === "undefined" || token === "") {
      next({ path: "/login" });
    } else {
      next(); 
    }
  } else {
    next(); 
  }
});



export default router;
