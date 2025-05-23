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
            <v-col cols="12">
              <h1 class="titulo">Olá, {{ nomeUsuario }}!</h1>
              <h4 class="subtitulo">
                Bem-vindo(a) ao painel de indicadores do CRM.
              </h4>
            </v-col>
          </v-row>
          <v-row>
            <v-col
              cols="12"
              md="8"
              class="pt-8 d-flex align-center flex-column"
            >
              <h4>Seus atendimentos do dia</h4>
              <TabelaGeral
                :headers="headers"
                :items="atendimentos"
                height="60vh"
                :mostrarAcoes="false"
              >
                <template #botoesExtras="{ item: atendimento }">
                  <v-tooltip bottom>
                    <template #activator="{ on, attrs }">
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
            <v-col
              cols="12"
              md="4"
              class="d-flex align-center flex-column pt-8"
            >
              <h4 class="mb-2">Seus indicadores</h4>
              <v-card class="pa-2 mt-0 cardIndicadores" width="80%">
                <v-card-text
                  class="d-flex align-center flex-column textoIndicadores"
                >
                  <h4 class="pb-4">Total de atendimentos em aberto</h4>
                  <h1 class="resultadoIndicadores">{{ totalEmAberto }}</h1>
                </v-card-text>
              </v-card>

              <v-card class="pa-2 mt-6 cardIndicadores" width="80%">
                <v-card-text
                  class="d-flex align-center flex-column textoIndicadores"
                >
                  <h4 class="pb-4">Índice de solução no primeiro contato</h4>
                  <h1 class="resultadoIndicadores">
                    {{ indiceSolucoesPrimeiroContato }}%
                  </h1>
                </v-card-text>
              </v-card>

              <v-card class="pa-2 mt-6 cardIndicadores" width="80%">
                <v-card-text
                  class="d-flex align-center flex-column textoIndicadores"
                >
                  <h4 class="pb-4">Total de atendimentos no mês atual</h4>
                  <h1 class="resultadoIndicadores">{{ totalNoMes }}</h1>
                </v-card-text>
              </v-card>
            </v-col>
          </v-row>
        </v-col>
      </v-row>
    </v-container>
  </v-main>
</template>

<script>
import TabelaGeral from "@/components/Tabela/TabelaGeral.vue";
import { icons } from "@/constants/icons";
import atendimentoService from "@/services/atendimentoService";
import storage from "@/utils/storage";

export default {
  name: "TelaInicial",
  components: {
    TabelaGeral,
  },
  data() {
    return {
      icons,
      nomeUsuario: "",
      totalEmAberto: 0,
      indiceSolucoesPrimeiroContato: 0,
      totalNoMes: 0,
      isLoading: true,
      atendimentos: [],
      headers: [
        { text: "Código", value: "idFormatado", width: "8vw" },
        {
          text: "Tipo de atendimento",
          value: "tipoAtendimentoDescricao",
          width: "14vw",
        },
        { text: "Pessoa", value: "pessoaNome", width: "18vw" },
        {
          text: "Data próximo contato",
          value: "dataProximoContatoFormatada",
          align: "center",
          width: "15vw",
        },
        { text: "Ações", value: "acoes", sortable: false },
      ],
    };
  },
  async mounted() {
    this.nomeUsuario = (localStorage.getItem("usuario") || "").toUpperCase();
    await this.carregarAtendimentos();
  },
  methods: {
    async carregarAtendimentos() {
      this.isLoading = true;
      try {
        const usuarioId = storage.obterUsuarioIdDoToken();
        if (!usuarioId) {
          console.warn("Usuário não logado ou token inválido.");
          return;
        }

        const hoje = new Date();
        const hojeISO = hoje.toISOString().split("T")[0];
        const mesAtual = hoje.getMonth();
        const anoAtual = hoje.getFullYear();

        const todosAtendimentos = await atendimentoService.obterTodos({
          usuarioProximoContatoId: usuarioId,
        });

        const atendimentosAbertos = todosAtendimentos.filter(
          (a) => a.statusItem === "Aberto"
        );
        this.totalEmAberto = atendimentosAbertos.length;

        const totalAtendimentos = todosAtendimentos.length;
        const atendimentosEncerradosMesmoDia = todosAtendimentos.filter((a) => {
          if (a.dataCadastro && a.dataInativacao) {
            const dataCadastro = a.dataCadastro.split("T")[0];
            const dataInativacao = a.dataInativacao.split("T")[0];
            return dataCadastro === dataInativacao;
          }
          return false;
        });
        this.indiceSolucoesPrimeiroContato =
          totalAtendimentos > 0
            ? (
                (atendimentosEncerradosMesmoDia.length / totalAtendimentos) *
                100
              ).toFixed(1)
            : 0;

        const atendimentosMesAtual = todosAtendimentos.filter((a) => {
          if (!a.dataCadastro) return false;
          const dataCadastro = new Date(a.dataCadastro);
          return (
            dataCadastro.getMonth() === mesAtual &&
            dataCadastro.getFullYear() === anoAtual
          );
        });
        this.totalNoMes = atendimentosMesAtual.length;

        this.atendimentos = atendimentosAbertos.filter((a) => {
          if (!a.proximoContato?.data) return false;
          return a.proximoContato.data.split("T")[0] <= hojeISO;
        });
      } catch (error) {
        console.error("Erro ao carregar atendimentos:", error);
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
  },
};
</script>

<style scoped>
.titulo {
  color: var(--text-primary);
}

.subtitulo {
  font-weight: 500;
}

.cardIndicadores {
  border-radius: 10px;
  border-top: 4px solid var(--primary);
}
</style>
