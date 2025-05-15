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

export default router;
