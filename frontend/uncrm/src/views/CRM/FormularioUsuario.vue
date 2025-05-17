<template>
  <v-main class="pa-0">
    <v-container fluid>
      <v-row>
        <v-col cols="12" md="12" class="pa-6">
          <v-row>
            <v-col
              cols="12"
              md="7"
              sm="12"
              class="d-flex align-center justify-start"
            >
              <breadcrumbs />
            </v-col>
          </v-row>

          <v-form
            ref="form"
            lazy-validation
            class="pa-5"
          >
            <v-col cols="12">
              <v-row class="d-flex align-center">
                <v-col cols="12" md="8" sm="12">
                  <v-text-field
                    elevation="0"
                    :rules="[(v) => !!v || 'O nome é obrigatório']"
                    label="Nome completo"
                    required
                    outlined
                    color="secondary"
                  />
                </v-col>
                
                <v-col cols="12" md="4" sm="12">
              <v-select
                label="Cargo"
                :items="tipoTitulos"
                item-text="nome"
                item-value="id"
                color="var(--color-default)"
                :rules="[
                  (v) =>
                    (v !== undefined && v !== null) ||
                    'O tipo de título é obrigatório',
                ]"
                required
                outlined
              />
            </v-col>
               
              </v-row>
              <v-row>
                <v-col cols="12" md="8" sm="12">
                  <v-text-field
                    elevation="0"
                    :rules="[(v) => !!v || 'O login é obrigatório']"
                    label="Login"
                    required
                    outlined
                    color="secondary"
                  />
                </v-col>
                 <v-col cols="12" md="4" sm="12">
                  <v-text-field
                    :append-icon="show ? 'mdi-eye' : 'mdi-eye-off'"
                    :type="show ? 'text' : 'password'"
                    outlined
                    @click:append="show = !show"
                    label="Senha"
                    :rules="[(v) => !!v || 'A senha é obrigatória']"
                    color="secondary"
                  ></v-text-field>
                </v-col>
              </v-row>
            </v-col>
            <v-row>
            </v-row>
          </v-form>
        </v-col>
      </v-row>
    </v-container>
  </v-main>
</template>

<script>
import Breadcrumbs from "@/components/Breadcrumbs.vue";

export default {
  name: "FormularioUsuario",
  components: {
    Breadcrumbs,
  },
  data() {
    return {
        show: false,
    };
  },
};
</script>

<style scoped></style>
