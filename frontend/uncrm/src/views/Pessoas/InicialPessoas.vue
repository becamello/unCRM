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
                  >Cadastrar pessoa
                </BotaoBase>
              </div>
            </v-col>
          </v-row>
          <tabela-geral
            :headers="headers"
            :items="pessoas"
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
                      v-model="pessoa.nome"
                      :rules="[(v) => !!v || 'O nome é obrigatório']"
                      label="Nome"
                      counter="50"
                      maxlength="50"
                      required
                      outlined
                      color="secondary"
                    />
                  </v-col>
                  <v-col cols="12" md="4" sm="12" class="pa-0 px-2">
                    <v-text-field
                      v-model="pessoa.nomeCurto"
                      outlined
                      @click:append="show = !show"
                      label="Nome curto"
                      :rules="[(v) => !!v || 'O nome curto é obrigatório']"
                      counter="30"
                      maxlength="30"
                      color="secondary"
                    ></v-text-field>
                  </v-col>
                </v-row>
                <v-row>
                  <v-col cols="12" md="8" sm="12" class="pa-0 px-2">
                    <v-text-field
                      v-model="pessoa.cpfCnpj"
                      :rules="[(v) => !!v || 'O CPF/CNPJ é obrigatório']"
                      counter="14"
                      maxlength="14"
                      label="CPF/CNPJ"
                      required
                      outlined
                      color="secondary"
                    />
                  </v-col>
                  <v-col cols="12" md="4" sm="12" class="pa-0 px-2">
                    <v-select
                      label="Tipo de pessoa"
                      v-model="pessoa.tipoPessoa"
                      :items="tipoPessoa"
                      item-text="nome"
                      item-value="id"
                      color="secondary"
                      item-color="secondary"
                      :rules="[
                        (v) =>
                          (v !== undefined && v !== null) ||
                          'O tipo de pessoa é obrigatório',
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
import { carregarTodasPessoas } from "@/utils/pessoas";

import Pessoa from "@/models/Pessoa";
import pessoaService from "@/services/pessoaService";

export default {
  name: "InicialPessoas",
  inject: ["showToast"],
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
      pessoa: new Pessoa(),
      modoCadastro: true,
      tipoPessoa: [
        { id: 0, nome: "Pessoa física" },
        { id: 1, nome: "Pessoa jurídica" },
      ],
      headers: [
        { text: "Código", value: "idFormatado", width: "8%" },
        { text: "Nome", value: "nome", width: "32%" },
        { text: "Nome curto", value: "nomeCurto", width: "18%" },
        { text: "CPF/CNPJ", value: "cpfCnpjFormatado", width: "18%" },
        { text: "Tipo de pessoa", value: "tipoPessoaDescricao", width: "12%" },
        { text: "Data cadastro", value: "dataCadastroFormatada", width: "18%" },
        { text: "Status", value: "statusItem", align: "center", width: "12%" },
        { text: "Ações", value: "acoes", sortable: false },
      ],
      pessoas: [],
      acoes: [
        {
          label: "Editar",
          icon: icons.editar,
          handler: (pessoa) => {
            pessoaService
              .obterPorId(pessoa.id)
              .then((response) => {
                this.pessoa = new Pessoa(response.data);
                this.modoCadastro = false;
                this.tituloModal = "Editar pessoa";
                this.modalVisivel = true;
                this.pessoa.senha = "";
              })
              .catch((error) => {
                console.error("Erro ao obter pessoa:", error);
                this.showToast(
                  "Erro",
                  "Erro ao carregar usuário para edição",
                  "error"
                );
              });
          },
          disabled: (pessoa) => pessoa.statusItem === "Inativo",
        },
        {
          label: "Inativar pessoa",
          icon: icons.inativar,
          handler: (pessoa) => {
            this.inativarPessoa(pessoa);
          },
          disabled: (pessoa) => pessoa.statusItem === "Inativo",
        },
      ],
      isLoading: true,
    };
  },
  async mounted() {
    Promise.all([(this.pessoas = await carregarTodasPessoas())]).finally(
      () => (this.isLoading = false)
    );
  },
  methods: {
    abrirModalCadastro() {
      this.tituloModal = "Cadastrar pessoa";
      this.pessoa = new Pessoa();
      this.modoCadastro = true;
      this.modalVisivel = true;
    },
    async salvar() {
      const isValid = this.$refs.form.validate();
      if (!isValid) return;

      this.isLoading = true;
      try {
        if (this.modoCadastro) {
          await pessoaService.cadastrar(this.pessoa);
          this.showToast(
            "Sucesso!",
            "Pessoa cadastrada com sucesso.",
            "success"
          );
        } else {
          await pessoaService.atualizar(this.pessoa);
          this.showToast("Sucesso!", "Pessoa editada com sucesso.", "success");
        }
        this.modalVisivel = false;
        this.pessoa = new Pessoa();
        this.pessoas = await carregarTodasPessoas();
      } catch (error) {
        console.error(error);
        this.showToast("Erro", "Não foi possível salvar a pessoa.", "error");
      } finally {
        this.isLoading = false;
      }
    },
    cancelar() {
      this.modalVisivel = false;
      this.pessoa = new Pessoa();
      this.$nextTick(() => {
        if (this.$refs.form) {
          this.$refs.form.resetValidation();
        }
      });
    },
    async inativarPessoa(pessoa) {
      this.isLoading = true;
      try {
        await pessoaService.inativar(pessoa);
        this.pessoas = await carregarTodasPessoas();
        this.showToast("Sucesso!", "Pessoa inativada com sucesso.", "success");
      } catch (error) {
        console.error(error);
        this.showToast("Erro", "Não foi possível inativar a pessoa.", "error");
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
