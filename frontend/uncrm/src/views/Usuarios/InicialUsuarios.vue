<template>
  <v-overlay v-if="isLoading" absolute>
    <v-progress-circular
      indeterminate
      color="var(--primary-color)"
      size="30"
    ></v-progress-circular>
  </v-overlay>
  <v-main class="pa-0" v-else>
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
            <v-col
              cols="12"
              md="5"
              sm="12"
              class="d-flex align-center justify-end"
            >
              <div class="botoes">
                <BotaoBase
                  variante="primario"
                  :iconeBtn="icons.adicionar"
                  @click="abrirModalCadastro"
                  >Cadastrar usuário
                </BotaoBase>
              </div>
            </v-col>
          </v-row>
          <tabela-geral
            :headers="headers"
            :items="usuarios"
            :actions="acoes"
            height="72vh"
          ></tabela-geral>
        </v-col>
      </v-row>

      <ModalFormulario
        v-model="modalVisivel"
        :titulo-modal="tituloModal"
        @salvar="salvar"
        @cancelar="cancelar"
        width="50%"
      >
        <v-row>
          <v-col cols="12" md="12">
            <v-form ref="form" lazy-validation>
              <v-col cols="12">
                <v-row>
                  <v-col cols="12" md="8" sm="12" class="pa-0 px-2">
                    <v-text-field
                      v-model="usuario.login"
                      :rules="[(v) => !!v || 'O login é obrigatório']"
                      label="Login"
                      required
                      outlined
                      color="secondary"
                    />
                  </v-col>
                  <v-col cols="12" md="4" sm="12" class="pa-0 px-2">
                    <v-text-field
                      v-model="usuario.senha"
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
                <v-row>
                  <v-col cols="12" md="8" sm="12" class="pa-0 px-2">
                    <v-text-field
                      v-model="usuario.nome"
                      :rules="[(v) => !!v || 'O nome é obrigatório']"
                      label="Nome completo"
                      required
                      outlined
                      color="secondary"
                    />
                  </v-col>
                  <v-col cols="12" md="4" sm="12" class="pa-0 px-2">
                    <v-select
                      label="Cargo"
                      v-model="usuario.cargo"
                      :items="cargosUsuario"
                      item-text="nome"
                      item-value="id"
                      color="secondary"
                      item-color="secondary"
                      :rules="[
                        (v) =>
                          (v !== undefined && v !== null) ||
                          'O cargo é obrigatório',
                      ]"
                      required
                      outlined
                    />
                  </v-col>
                </v-row>
              </v-col>
            </v-form>
          </v-col>
        </v-row>
      </ModalFormulario>
    </v-container>
  </v-main>
</template>

<script>
import Breadcrumbs from "@/components/Breadcrumbs.vue";
import BotaoBase from "@/components/Base/BotaoBase.vue";
import TabelaGeral from "@/components/Tabela/TabelaGeral.vue";
import ModalFormulario from "@/components/Base/ModalFormulario.vue";

import { icons } from "@/constants/icons";
import { carregarTodosUsuarios } from "@/utils/usuarios";

import Usuario from "@/models/Usuario";
import usuarioService from "@/services/usuarioService";

export default {
  name: "InicialUsuarios",
  components: {
    Breadcrumbs,
    BotaoBase,
    TabelaGeral,
    ModalFormulario,
  },
  data() {
    return {
      icons,
      show: false,
      modalVisivel: false,
      tituloModal: "",
      usuario: new Usuario(),
      modoCadastro: true,
      cargosUsuario: [
        { id: 0, nome: "Gerente" },
        { id: 1, nome: "Supervisor" },
        { id: 2, nome: "Atendente" },
      ],
      headers: [
        { text: "Código", value: "id", width: "8%" },
        { text: "Login", value: "login", width: "18%" },
        { text: "Nome", value: "nome", width: "32%" },
        { text: "Cargo", value: "cargoDescricao", width: "18%" },
        { text: "Data cadastro", value: "dataCadastroFormatada", width: "12%" },
        { text: "Status", value: "statusItem", align: "center", width: "12%" },
        { text: "Ações", value: "acoes", sortable: false },
      ],
      usuarios: [],
      acoes: [
        {
          label: "Editar",
          icon: icons.editar,
          handler: (usuario) => {
            usuarioService
              .obterPorId(usuario.id)
              .then((response) => {
                this.usuario = new Usuario(response.data);
                this.modoCadastro = false;
                this.tituloModal = "Editar usuário";
                this.modalVisivel = true;
                this.usuario.senha = "";
              })
              .catch((error) => {
                console.error("Erro ao obter usuario:", error);
                // this.$toast.error("Erro ao carregar usuario para edição");
              });
          },
          disabled: (usuario) => usuario.statusItem === "Inativo",
        },
        {
          label: "Inativar usuário",
          icon: icons.inativar,
          handler: (usuario) => {
            this.inativarUsuario(usuario);
          },
          disabled: (usuario) => usuario.statusItem === "Inativo",
        },
      ],
      isLoading: true,
    };
  },
  async mounted() {
    Promise.all([(this.usuarios = await carregarTodosUsuarios())]).finally(
      () => (this.isLoading = false)
    );
  },
  methods: {
    abrirModalCadastro() {
      this.tituloModal = "Cadastrar usuário";
      this.usuario = new Usuario();
      this.modoCadastro = true;
      this.modalVisivel = true;
    },
    async salvar() {
      const isValid = this.$refs.form.validate();
      if (!isValid) return;

      this.isLoading = true;
      try {
        if (this.modoCadastro) {
          await usuarioService.cadastrar(this.usuario);
        } else {
          await usuarioService.atualizar(this.usuario);
        }
        this.modalVisivel = false;
        this.usuario = new Usuario();
        this.usuarios = await carregarTodosUsuarios();
      } catch (error) {
        console.error("Erro ao salvar usuário:", error);
        this.$toast.error("Erro ao salvar usuário.");
      } finally {
        this.isLoading = false;
      }
    },
    cancelar() {
      this.modalVisivel = false;
      this.usuario = new Usuario();
      this.$nextTick(() => {
        if (this.$refs.form) {
          this.$refs.form.resetValidation();
        }
      });
    },
    async inativarUsuario(usuario) {
      this.isLoading = true;
      try {
        await usuarioService.inativar(usuario);
        this.usuarios = await carregarTodosUsuarios();
      } catch (error) {
        console.error(error);
      }
      this.isLoading = false;
    },
  },
};
</script>

<style scoped>
.botoes {
  display: flex;
  flex-wrap: wrap;
  gap: 10px;
}
</style>
