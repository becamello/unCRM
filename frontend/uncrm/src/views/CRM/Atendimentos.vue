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
                <v-list-item-title class="titulo-filtro"
                  >Filtro</v-list-item-title
                >
              </v-list-item-content>
            </v-list-item>
          </v-list>

          <v-list-item>
            <v-list-item-content>
              <v-form>
                <div class="d-flex align-center mb-5">
                  <p class="mb-0 mr-1">Cadastro</p>
                  <v-divider />
                </div>
                <DataPicker
                  dense
                  label="Data inicial"
                  v-model="filtro.dataInicialCadastro"
                />
                <DataPicker
                  dense
                  label="Data final"
                  v-model="filtro.dataFinalCadastro"
                />
                <v-autocomplete
                  dense
                  outlined
                  deletable-chips
                  multiple
                  small-chips
                  v-model="filtro.usuarioCriadorId"
                  :items="usuarios"
                  item-value="id"
                  item-text="login"
                  label="Usuário"
                  color="secondary"
                  item-color="secondary"
                />
                <div class="d-flex align-center mb-5">
                  <p class="mb-0 mr-1">Próximo contato</p>
                  <v-divider />
                </div>
                <DataPicker
                  dense
                  label="Data inicial"
                  v-model="filtro.dataInicialProximoContato"
                />
                <DataPicker
                  dense
                  label="Data final"
                  v-model="filtro.dataFinalProximoContato"
                />
                <v-autocomplete
                  dense
                  outlined
                  deletable-chips
                  multiple
                  small-chips
                  v-model="filtro.usuarioProximoContatoId"
                  :items="usuarios"
                  item-value="id"
                  item-text="login"
                  label="Usuário"
                  color="secondary"
                  item-color="secondary"
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
                    v-model="atendimento.proximoContato.data"
                    label="Data próximo contato"
                    required
                    :rules="[
                      (v) =>
                        (v !== undefined && v !== null) ||
                        'A data de próximo contato é obrigatória',
                    ]"
                  />
                </v-col>
                <v-col cols="12" md="6" sm="6" class="pa-0 px-2">
                  <v-autocomplete
                    v-model="atendimento.proximoContato.usuario"
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

import { icons } from "@/constants/icons";

import atendimentoService from "@/services/atendimentoService";
import { carregarTodosUsuariosAtivos } from "@/utils/usuarios";
import { carregarTodosTiposAgendamento } from "@/utils/tipoAtendimento";
import { carregarTodasPessoasAtivas } from "@/utils/pessoas";
import storage from "@/utils/storage";

import Atendimento from "@/models/Atendimento";

export default {
  name: "CRMAtendimentos",
  components: {
    TabelaGeral,
    Breadcrumbs,
    BotaoBase,
    DataPicker,
    ModalFormulario,
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
              alert("Apenas gerentes podem reabrir atendimentos.");
              return;
            }

            this.isLoading = true;
            try {
              await atendimentoService.reabrir(atendimento.id);
              await this.carregarAtendimentos();
            } catch (error) {
              console.error("Erro ao reabrir atendimento:", error);
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
    console.log(usuario);
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
      }
    },
    async filtrarAtendimentos(){
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

      this.isLoading = true;
      try {
        let usuarioLogadoId = storage.obterUsuarioIdDoToken();

        if (this.modoCadastro) {
          await atendimentoService.criar(this.atendimento, usuarioLogadoId);
        } else {
          await atendimentoService.atualizar(this.atendimento);
        }
        this.modalVisivel = false;
        this.atendimento = new Atendimento();
        await this.carregarAtendimentos();
      } catch (error) {
        console.error("Erro ao salvar atendimento:", error);
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
      }
    }
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
