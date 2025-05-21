<template>
  <v-main class="pa-0 main403">
    <v-container fluid class="fill-height">
      <v-row class="d-flex align-center ma-10">
        <v-col cols="12" col="12" md="6">
          <img
            :src="require('@/assets/img/403/image-403.png')"
            alt="Erro 403"
            draggable="false"
          />
        </v-col>
        <v-col cols="12" col="12" md="6">
          <img
            :src="require('@/assets/img/403/text-403.png')"
            alt="403 - Acesso negado, sem permissão para esta página"
            draggable="false"
          />
        </v-col>
      </v-row>
    </v-container>
  </v-main>
</template>

<script>
export default {
  name: "StatusCodePage403",
};
</script>

<style scoped>
.fill-height {
  height: 100vh;
}

.main403 {
  background-color: var(--primary-color)!important;
}
</style>
