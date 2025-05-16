<template>
  <v-app>
    <v-main>
      <MenuLateral v-if="this.$router.currentRoute.name != 'Login'" />
      <router-view/>
      
    </v-main>
  </v-app>
</template>

<script>
import MenuLateral from "./components/Menu/MenuLateral.vue";

export default {
  name: 'App',

   components: {
    MenuLateral,
  }
};
</script>


<style>
* {
  box-sizing: border-box;
}

html {
  height: 100%;
  margin: 0;
  overflow-y: auto !important;
}
#app {
  min-height: 100vh;
}

.v-toast__item {
  font-family: "Roboto", sans-serif !important;
}
</style>
