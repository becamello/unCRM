<template>
  <v-main class="pa-0 main404">
    <v-container fluid class="fill-height ">
      <v-row class="d-flex align-center ma-10">
        <v-col cols="12" col="12" md="6">
          <img
            :src="require('@/assets/img/404/image-404.png')"
            alt="Erro 403"
            draggable="false"
          />
        </v-col>
        <v-col cols="12" col="12" md="6">
          <img
            :src="require('@/assets/img/404/text-404.png')"
            alt="404 - Não encontrado, essa página não existe"
            draggable="false"
          />
        </v-col>
      </v-row>
    </v-container>
  </v-main>
</template>

<script>
export default {
  name: "StatusCodePage404",
};
</script>

<style scoped>
.fill-height {
  height: 100vh;
}

.main404 {
  background-image: url('~@/assets/img/404/background-404.png');
  background-color: var(--primary-color);
  background-size: cover;
  background-position: center;
  background-repeat: no-repeat;
}
</style>
