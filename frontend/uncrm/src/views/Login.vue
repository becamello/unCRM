<template>
  <v-container fluid class="container-login">
    <v-row class="fill-height d-flex align-center justify-center">
      <v-col
        cols="12"
        md="6"
        class="d-none d-md-flex justify-center align-center"
      >
        <img
          src="../assets/img/Logotipo.png"
          alt="UnCRM, o únicoCRM que você precisa."
          style="width: 70%"
        />
      </v-col>
      <v-col cols="12" md="6" class="d-flex align-center justify-center">
        <v-card elevation="5" class="card-form-login">
          <v-card-title class="login-title">BEM-VINDO!</v-card-title>
          <v-card-text>
            <v-text-field
              outlined
              color="var(--neutral-dark)"
              label="Login"
              placeholder="Insira seu login de acesso"
              v-model="usuario.login"
            ></v-text-field>

            <v-text-field
              :append-icon="show ? 'mdi-eye' : 'mdi-eye-off'"
              :type="show ? 'text' : 'password'"
              outlined
              @click:append="show = !show"
              name="Senha"
              label="Senha"
              color="var(--neutral-dark)"
              v-model="usuario.senha"
            ></v-text-field>
          </v-card-text>
          <v-card-actions class="d-flex align-center justify-center">
            <v-btn
              color="var(--primary-color)"
              dark
              class="btn-login"
              @click="login"
              :disabled="isLoading"
            >
              ENTRAR
            </v-btn>
          </v-card-actions>

          <v-overlay v-if="isLoading" absolute>
            <v-progress-circular
              indeterminate
              color="var(--primary-color)"
              size="30"
            ></v-progress-circular>
          </v-overlay>
        </v-card>
      </v-col>
    </v-row>
  </v-container>
</template>

<script>
import usuarioService from "@/services/usuarioService";
import utilStorage from "@/utils/storage";

export default {
  name: "LoginPage",

  data() {
    return {
      show: false,
      usuario: {
        login: "",
        senha: "",
      },
      isLoading: false,
    };
  },
  methods: {
    login() {
      if (!this.usuario.login || !this.usuario.senha) {
        console.log("Insira dados");
        return;
      }

      this.isLoading = true;

      usuarioService
        .login(this.usuario.login, this.usuario.senha)
        .then((response) => {
          this.isLoading = false;

          utilStorage.salvarStorage(response.data.login);
          utilStorage.salvarTokenNaStorage(response.data.token);
          this.$router.push({ name: "Inicial" });
        })
        .catch((error) => {
          console.error(error);
          this.isLoading = false;
          console.log("Erro ao realizar login. Dados inválidos");
        });
    },
  },
};
</script>

<style scoped>
.container-login {
  background-color: var(--primary-color);
  height: 100vh;
}

.card-form-login {
  width: 36vw;
  padding: 3rem;
  display: flex;
  flex-direction: column;
  align-items: center;
  border-radius: 2rem;
}

.login-title {
  font-size: 2.5rem;
  color: var(--primary-color);
  font-weight: 900;
  padding-bottom: 3rem;
}

.btn-login {
  width: 200px;
  font-size: 18px;
}

@media (max-width: 600px) {
  .card-form-login {
    padding: 2rem;
    width: 95%;
  }
}
</style>
