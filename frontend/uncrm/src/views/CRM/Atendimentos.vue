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
                  >Novo atendimento</BotaoBase
                >
                <BotaoBase
                  variante="secundario"
                  :iconeBtn="icons.filtro"
                  @click="abrirFiltro"
                  >Filtros
                </BotaoBase>
              </div>
            </v-col>
          </v-row>
          <TabelaGeral
            :headers="headers"
            :items="atendimentos"
            :actions="acoes"
            action-icon="mdi-dots-vertical"
            height="72vh"
          >
            <template v-slot:botoesExtras="{ item: atendimento }">
              <v-tooltip bottom>
                <template v-slot:activator="{ on, attrs }">
                  <v-btn
                    icon
                    v-bind="attrs"
                    v-on="on"
                    @click="historicoAtendimento(atendimento)"
                  >
                    <v-icon small>{{ icons.historico }}</v-icon>
                  </v-btn>
                </template>
                <span>Histórico</span>
              </v-tooltip>
            </template>
          </TabelaGeral>
        </v-col>
        <v-navigation-drawer
          v-model="drawer"
          bottom
          fixed
          temporary
          right
          :width="360"
        >
          <v-list nav>
            <v-list-item>
              <v-list-item-content>
                <v-list-item-title class="d-flex justify-center titulo-filtro"
                  >Filtros de atendimentos</v-list-item-title
                >
              </v-list-item-content>
            </v-list-item>
          </v-list>

          <v-list-item>
            <v-list-item-content>
              <v-form>
                <div class="d-flex align-center mb-5">
                  <p class="mb-0 mr-1">Próximo contato</p>
                  <v-divider />
                </div>
                <DataPicker
                  label="Data inicial"
                  v-model="filtro.dataInicialProximoContato"
                  :max="filtro.dataFinalProximoContato || null"
                  dense
                />
                <DataPicker
                  label="Data final"
                  v-model="filtro.dataFinalProximoContato"
                  :min="filtro.dataInicialProximoContato || null"
                  dense
                />
                <AutocompleteBase
                  v-model="filtro.usuarioProximoContatoId"
                  :items="usuarios"
                  label="Usuário"
                  item-value="id"
                  item-text="login"
                />
                <div class="d-flex align-center mb-5">
                  <p class="mb-0 mr-1">Cadastro</p>
                  <v-divider />
                </div>
                <DataPicker
                  label="Data inicial"
                  v-model="filtro.dataInicialCadastro"
                  :max="filtro.dataFinalCadastro || null"
                  dense
                />
                <DataPicker
                  label="Data final"
                  v-model="filtro.dataFinalCadastro"
                  :min="filtro.dataInicialCadastro || null"
                  dense
                />
                <AutocompleteBase
                  v-model="filtro.usuarioCriadorId"
                  :items="usuarios"
                  label="Usuário"
                  item-value="id"
                  item-text="login"
                />
                <div class="d-flex align-center mb-5">
                  <p class="mb-0 mr-1">Pessoa</p>
                  <v-divider />
                </div>
                <AutocompleteBase
                  v-model="filtro.pessoaId"
                  :items="pessoas"
                  label="Pessoas"
                  item-value="id"
                  item-text="nome"
                />
              </v-form>
            </v-list-item-content>
          </v-list-item>

          <template #append>
            <v-list-item>
              <v-list-item-content>
                <v-row>
                  <v-col cols="6" class="d-flex justify-center">
                    <BotaoBase
                      variante="secundario"
                      width="100%"
                      @click="limparFiltro"
                      >Limpar</BotaoBase
                    >
                  </v-col>
                  <v-col cols="6" class="d-flex justify-center">
                    <BotaoBase
                      variante="primario"
                      width="100%"
                      @click="filtrarAtendimentos"
                      >Filtrar</BotaoBase
                    >
                  </v-col>
                </v-row>
              </v-list-item-content>
            </v-list-item>
          </template>
        </v-navigation-drawer>
      </v-row>

      <ModalFormulario
        v-model="modalVisivel"
        :titulo-modal="tituloModal"
        @salvar="salvar"
        @cancelar="cancelar"
        width="60%"
      >
        <v-row>
          <v-col cols="12" md="12" class="px-6">
            <v-form ref="form" lazy-validation>
              <v-row>
                <v-col cols="12" md="6" sm="12" class="pa-0 px-2">
                  <v-select
                    v-model="atendimento.tipoAtendimentoId"
                    label="Tipo de atendimento"
                    :items="tipoAtendimentos"
                    item-value="id"
                    item-text="descricao"
                    color="secondary"
                    item-color="secondary"
                    :rules="[
                      (v) =>
                        (v !== undefined && v !== null) ||
                        'O tipo de atendimento é obrigatório',
                    ]"
                    required
                    outlined
                  />
                </v-col>
                <v-col cols="12" md="6" sm="12" class="pa-0 px-2">
                  <v-select
                    v-model="atendimento.pessoaId"
                    label="Pessoa"
                    :items="pessoas"
                    item-text="nome"
                    item-value="id"
                    color="secondary"
                    item-color="secondary"
                    :rules="[
                      (v) =>
                        (v !== undefined && v !== null) ||
                        'A pessoa é obrigatória',
                    ]"
                    required
                    outlined
                  />
                </v-col>
              </v-row>

              <v-row>
                <v-col cols="12" md="12" sm="12" class="pa-0 px-2">
                  <v-textarea
                    v-model="atendimento.parecer"
                    :rules="[(v) => !!v || 'O parecer é obrigatório']"
                    label="Descrição do parecer"
                    required
                    outlined
                    clearable
                    rows="6"
                    no-resize
                    color="secondary"
                  />
                </v-col>
              </v-row>
              <v-row>
                <v-col cols="12" md="6" sm="6" class="pa-0 px-2">
                  <DataPicker
                    v-model="tempProximoContatoData"
                    label="Data próximo contato"
                    required
                    :min="hoje"
                    :rules="[
                      (v) =>
                        (v !== undefined && v !== null) ||
                        'A data de próximo contato é obrigatória',
                    ]"
                  />
                </v-col>
                <v-col cols="12" md="6" sm="6" class="pa-0 px-2">
                  <v-autocomplete
                    v-model="tempProximoContatoUsuario"
                    outlined
                    required
                    :items="usuarios"
                    item-value="id"
                    item-text="login"
                    label="Usuário próximo contato"
                    color="secondary"
                    item-color="secondary"
                    :rules="[
                      (v) =>
                        (v !== undefined && v !== null) ||
                        'O usuário de próximo contato é obrigatório',
                    ]"
                  />
                </v-col>
              </v-row>
            </v-form>
          </v-col>
        </v-row>
      </ModalFormulario>
    </v-container>
  </v-main>
</template>

<script>
import TabelaGeral from "@/components/Tabela/TabelaGeral.vue";
import Breadcrumbs from "@/components/Breadcrumbs.vue";
import BotaoBase from "@/components/Base/BotaoBase.vue";
import DataPicker from "@/components/Base/DataPicker.vue";
import ModalFormulario from "@/components/Base/ModalFormulario.vue";
import AutocompleteBase from "@/components/Base/AutocompleteBase.vue";

import { icons } from "@/constants/icons";

import atendimentoService from "@/services/atendimentoService";
import { carregarTodosUsuariosAtivos } from "@/utils/usuarios";
import { carregarTodosTiposAgendamento } from "@/utils/tipoAtendimento";
import { carregarTodasPessoasAtivas } from "@/utils/pessoas";
import storage from "@/utils/storage";

import Atendimento from "@/models/Atendimento";

export default {
  name: "CRMAtendimentos",
  inject: ["showToast"],
  components: {
    TabelaGeral,
    Breadcrumbs,
    BotaoBase,
    DataPicker,
    ModalFormulario,
    AutocompleteBase,
  },
  data() {
    return {
      icons,
      modalVisivel: false,
      atendimento: new Atendimento(),
      modoCadastro: true,
      tituloModal: "",
      isGerente: false,
      drawer: false,
      isLoading: true,
      hoje: new Date().toISOString().split("T")[0],
      tempProximoContatoData: null,
      tempProximoContatoUsuario: null,
      usuarios: [],
      tipoAtendimentos: [],
      pessoas: [],
      headers: [
        { text: "Código", value: "idFormatado", width: "8%" },
        {
          text: "Tipo de atendimento",
          value: "tipoAtendimentoDescricao",
          width: "18%",
        },
        { text: "Pessoa", value: "pessoaNome", width: "34%" },
        {
          text: "Próximo contato",
          value: "proximoContato.descricao",
          width: "20%",
        },
        { text: "Cadastro", value: "dataUsuarioCadastro", width: "20%" },
        { text: "Status", value: "statusItem", align: "center" },
        { text: "Ações", value: "acoes", sortable: false },
      ],
      atendimentos: [],
      acoes: [
        {
          label: "Editar atendimento",
          icon: icons.editar,
          handler: (atendimento) => {
            atendimentoService
              .obterPorId(atendimento.id)
              .then((response) => {
                this.atendimento = new Atendimento(response.data);
                this.modoCadastro = false;
                this.tempProximoContatoData = null;
                this.tempProximoContatoUsuario = null;
                this.tituloModal = "Editar atendimento";
                this.modalVisivel = true;
              })
              .catch((error) => {
                console.error("Erro ao obter atendimento:", error);
              });
          },
        },
        {
          label: "Alterar próximo contato",
          icon: icons.alterarProximoContato,
          //   handler: (item) => {
          //     console.log("Excluir", item);
          //   },
        },
        {
          label: "Reabrir atendimento",
          icon: icons.reabrir,
          disabled: (atendimento) => atendimento.status === 0,
          handler: async (atendimento) => {
            if (!this.isGerente) {
              this.showToast(
                "Acesso negado",
                "Apenas gerentes podem reabrir atendimentos",
                "warning"
              );
              return;
            }

            this.isLoading = true;
            try {
              await atendimentoService.reabrir(atendimento.id);
              await this.carregarAtendimentos();
              this.showToast(
                "Sucesso!",
                "O atendimento foi reaberto",
                "success"
              );
            } catch (error) {
              console.error("Erro ao reabrir atendimento:", error);
              this.showToast(
                "Erro",
                "Não foi possível reabrir o atendimento",
                "error"
              );
            } finally {
              this.isLoading = false;
            }
          },
        },
      ],
      filtro: {
        pessoaId: [],
        usuarioCriadorId: [],
        dataInicialCadastro: "",
        dataFinalCadastro: "",
        usuarioProximoContatoId: [],
        dataInicialProximoContato: "",
        dataFinalProximoContato: "",
      },
    };
  },
  async mounted() {
    const usuario = storage.obterCargoNaStorage();
    this.isGerente = usuario.isGerente;
    this.carregarAtendimentos();
    this.usuarios = await carregarTodosUsuariosAtivos();
    this.tipoAtendimentos = await carregarTodosTiposAgendamento();
    this.pessoas = await carregarTodasPessoasAtivas();
    this.isLoading = false;
  },

  methods: {
    async carregarAtendimentos() {
      try {
        const resultado = await atendimentoService.obterTodos(this.filtro);
        this.atendimentos = resultado.map((a) => new Atendimento(a));
      } catch (erro) {
        console.error("Erro ao carregar atendimentos:", erro);
        this.showToast("Erro", "Falha ao carregar atendimentos.", "error");
      }
    },
    async filtrarAtendimentos() {
      this.isLoading = true;
      await this.carregarAtendimentos();
      this.isLoading = false;
      this.drawer = false;
    },
    getNomeUsuario(id) {
      const usuario = this.usuarios.find((u) => u.id === id);
      return usuario ? usuario.nome : "Usuário desconhecido";
    },
    formatarProximoContato(proximoContato) {
      if (!proximoContato) return "";
      const nome = this.getNomeUsuario(proximoContato.usuario);
      const dataFormatada = new Date(proximoContato.data).toLocaleDateString();
      return `${dataFormatada} - ${nome}`;
    },
    abrirFiltro() {
      this.drawer = true;
    },
    abrirModalCadastro() {
      this.tituloModal = "Novo atendimento";
      this.atendimento = new Atendimento();
      this.modoCadastro = true;
      this.modalVisivel = true;
    },
    cancelar() {
      this.modalVisivel = false;
      this.atendimento = new Atendimento();
      this.$nextTick(() => {
        if (this.$refs.form) {
          this.$refs.form.resetValidation();
        }
      });
    },
    async salvar() {
      const isValid = this.$refs.form.validate();
      if (!isValid) return;

      this.atendimento.proximoContato = {
        data: this.tempProximoContatoData,
        usuario: this.tempProximoContatoUsuario,
      };

      this.isLoading = true;
      try {
        let usuarioLogadoId = storage.obterUsuarioIdDoToken();

        if (this.modoCadastro) {
          await atendimentoService.criar(this.atendimento, usuarioLogadoId);
          this.showToast("Sucesso!", "Atendimento criado", "success");
        } else {
          await atendimentoService.atualizar(this.atendimento);
          this.showToast("Sucesso!", "Atendimento editado", "success");
        }

        this.modalVisivel = false;
        this.atendimento = new Atendimento();
        await this.carregarAtendimentos();
      } catch (error) {
        console.error("Erro ao salvar atendimento:", error);
        this.showToast(
          "Erro",
          "Não foi possível salvar o atendimento",
          "error"
        );
      } finally {
        this.isLoading = false;
      }
    },
    historicoAtendimento(atendimento) {
      this.$router.push({
        name: "HistoricoAtendimento",
        params: { id: atendimento.id },
      });
    },
    limparFiltro() {
      this.filtro = {
        pessoaId: [],
        usuarioCriadorId: [],
        dataInicialCadastro: "",
        dataFinalCadastro: "",
        usuarioProximoContatoId: [],
        dataInicialProximoContato: "",
        dataFinalProximoContato: "",
      };
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

.titulo-filtro {
  color: var(--text-primary);
  font-size: 22px;
  font-weight: 500;
}
</style>
