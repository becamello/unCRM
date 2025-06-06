<template>
  <v-overlay v-if="isLoading" absolute>
    <v-progress-circular
      indeterminate
      color="var(--primary-color)"
      size="30"
    ></v-progress-circular>
  </v-overlay>
  <v-main class="pa-0" v-else>
    <v-container fluid class="fill-height">
      <v-row>
        <v-col cols="12" md="12" class="pa-6 pb-0">
          <v-row>
            <v-col
              cols="12"
              md="7"
              sm="6"
              class="d-flex align-center justify-start pb-1"
            >
              <breadcrumbs />
            </v-col>
          </v-row>
          <v-row class="mb-1">
            <v-col
              cols="12"
              md="3"
              sm="3"
              class="py-0"
              v-for="(info, index) in infosPrincipais()"
              :key="index"
            >
              <IconLabel :icon="icons[info.icon]" :label="info.label">
                <template v-if="info.isStatus">
                  <v-chip
                    :color="statusChipColor(info.valor)"
                    dark
                    x-small
                    outlined
                    class="chip-status"
                  >
                    {{ info.valor }}
                  </v-chip>
                </template>
                <template v-else>
                  {{ info.valor }}
                </template>
              </IconLabel>
            </v-col>
          </v-row>
        </v-col>
      </v-row>
      <v-row>
        <v-col cols="12" md="12">
          <div style="height: 50vh; overflow-y: auto">
            <ParecerCard
              v-for="parecer in atendimento.pareceres"
              :key="parecer.id"
              :parecer="parecer"
              :icons="icons"
              :usuario-logado-id="Number(usuarioLogadoId)"
              :atendimento-encerrado="atendimentoEncerrado"
              @editar="abrirEdicaoParecer"
            />
          </div>
        </v-col>
      </v-row>
      <v-form ref="formParecer" lazy-validation style="width: 100%">
        <v-row class="d-flex flex-sm-row align-sm-center">
          <v-col cols="10" md="11" sm="11">
            <v-textarea
              v-model="parecer.parecer"
              :rules="[(v) => !!v || 'O parecer é obrigatório']"
              counter="500"
              maxlength="500"
              :disabled="atendimentoEncerrado"
              label="Descrição do parecer"
              required
              outlined
              clearable
              rows="4"
              no-resize
              color="secondary"
            />
          </v-col>
          <v-col cols="2" md="1" sm="1">
            <v-tooltip bottom>
              <template v-slot:activator="{ on, attrs }">
                <span v-bind="attrs" v-on="on">
                  <v-btn
                    color="primary"
                    fab
                    elevation="0"
                    :small="$vuetify.breakpoint.smAndDown"
                    :disabled="atendimentoEncerrado"
                    @click="abrirModal"
                    ><v-icon>{{ icons.registrarParecer }}</v-icon></v-btn
                  >
                </span>
              </template>
              <span v-if="atendimentoEncerrado">
                Atendimentos encerrados não aceitam novos pareceres
              </span>
            </v-tooltip>
          </v-col>
        </v-row>
      </v-form>
      <ModalFormulario
        v-if="modalVisivel"
        v-model="modalVisivel"
        @cancelar="cancelar"
        @salvar="aoSalvarParecer"
        :titulo-modal="tituloModal"
        width="50%"
      >
        <v-overlay v-if="isSubmitting" absolute>
          <v-progress-circular indeterminate color="primary" size="30" />
        </v-overlay>
        <template v-if="estaEditandoParecer">
          <v-textarea
            v-model="parecerEditando.descricao"
            label="Descrição do parecer"
            outlined
            rows="5"
            clearable
            required
            counter="500"
            maxlength="500"
            :rules="[(v) => !!v || 'Descrição obrigatória']"
          />
        </template>

        <template v-else>
          <v-radio-group v-model="parecerAcao" class="mb-4">
            <v-radio label="Encerrar atendimento" value="encerrar"></v-radio>
            <v-radio label="Definir próximo contato" value="proximo"></v-radio>
          </v-radio-group>

          <div v-if="parecerAcao === 'proximo'">
            <DataPicker
              v-model="tempProximoContatoData"
              label="Data próximo contato"
              required
              :min="hoje"
            />
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
            />
          </div>
        </template>
      </ModalFormulario>
    </v-container>
  </v-main>
</template>


<script>
import Atendimento from "@/models/Atendimento";
import Parecer from "@/models/Parecer";

import atendimentoService from "@/services/atendimentoService";
import storage from "@/utils/storage";
import { carregarTodosUsuariosAtivos } from "@/utils/usuarios";

import Breadcrumbs from "@/components/Breadcrumbs.vue";
import ModalFormulario from "@/components/Base/ModalFormulario.vue";
import DataPicker from "@/components/Base/DataPicker.vue";
import IconLabel from "@/components/Atendimento/IconLabel.vue";
import ParecerCard from "@/components/Atendimento/ParecerCard.vue";
import { icons } from "@/constants/icons";

export default {
  inject: ["showToast"],
  components: {
    Breadcrumbs,
    ModalFormulario,
    DataPicker,
    IconLabel,
    ParecerCard,
  },
  data() {
    return {
      icons,
      usuarioLogadoId: storage.obterUsuarioIdDoToken(),
      atendimento: null,
      isLoading: true,
      
      parecerAcao: null,
      proximoContatoData: "",
      proximoContatoUsuario: "",
      isSubmitting: false,
      modalVisivel: false,
      usuarios: [],
      parecer: new Parecer(),
      parecerEditando: null,
      estaEditandoParecer: false,
      tempProximoContatoData: null,
      tempProximoContatoUsuario: null,
      hoje: new Date().toISOString().split("T")[0],
    };
  },
  async mounted() {
    Promise.all([
      this.carregarAtendimento(),
      (this.usuarios = await carregarTodosUsuariosAtivos()),
    ]).finally(() => {
      this.isLoading = false;
    });
  },
  computed: {
    atendimentoEncerrado() {
      return this.atendimento?.status === 1;
    },
  },
  methods: {
    async carregarAtendimento() {
      const id = this.$route.params.id;
      try {
        const response = await atendimentoService.obterPorId(id);
        const atendimentoData = response.data;

        atendimentoData.pareceres =
          atendimentoData.pareceres?.map((p) => new Parecer(p)) ?? [];

        this.atendimento = new Atendimento(atendimentoData);
      } catch (error) {
        console.error("Erro ao carregar atendimento:", error);
        this.showToast(
          "Erro",
          "Erro ao carregar os dados do atendimento. Tente novamente mais tarde.",
          "error"
        );
      }
    },
    infosPrincipais() {
      return [
        {
          icon: "codigoAtendimento",
          label: "Código",
          valor: this.atendimento?.idFormatado || "",
        },
        {
          icon: "tipoAtendimento",
          label: "Tipo atendimento",
          valor: this.atendimento?.tipoAtendimentoDescricao || "",
        },
        {
          icon: "usuarioGenerico",
          label: "Usuário de abertura",
          valor: this.atendimento?.usuarioCriadorLogin || "",
        },
        {
          icon: "usuarioGenerico",
          label: "Usuário próximo contato",
          valor: this.atendimento?.proximoContato?.usuarioLogin || "",
        },
        {
          icon: "pessoa",
          label: "Pessoa",
          valor: this.atendimento?.pessoaNome || "",
        },
        {
          icon: "status",
          label: "Status",
          valor: this.atendimento?.statusItem || "",
          isStatus: true,
        },
        {
          icon: "dataGenerico",
          label: "Data cadastro",
          valor: this.atendimento?.dataCadastroFormatada || "",
        },
        {
          icon: "dataGenerico",
          label: "Data próximo contato",
          valor: this.atendimento?.proximoContato?.dataFormatada || "",
        },
      ];
    },
    aoSalvarParecer() {
      if (this.estaEditandoParecer) {
        this.salvarEdicaoParecer();
      } else {
        this.confirmarAcaoParecer();
      }
    },
    abrirModal() {
      if (this.$refs.formParecer.validate()) {
        this.tempProximoContatoData = null;
        this.tempProximoContatoUsuario = null;
        this.modalVisivel = true;
        this.tituloModal = "Próximo passo:";
      }
    },
    async confirmarAcaoParecer() {
      if (!this.parecerAcao) {
        this.showToast(
          "Atenção!",
          "Selecione uma ação: encerrar atendimento ou definir próximo contato.",
          "warning"
        );
        return;
      }

      const parecerDescricao = this.parecer.parecer || this.parecer;
      const atendimentoId = this.atendimento.id;

      this.isSubmitting = true;

      try {
        if (this.parecerAcao === "encerrar") {
          await atendimentoService.registrarParecerComEncerramento(
            atendimentoId,
            { parecer: parecerDescricao }
          );
          this.showToast(
            "Sucesso!",
            "Parecer registrado e atendimento encerrado.",
            "success"
          );
        }

        if (this.parecerAcao === "proximo") {
          if (!this.tempProximoContatoUsuario || !this.tempProximoContatoData) {
            return;
          }
          this.atendimento.proximoContato.usuario =
            this.tempProximoContatoUsuario;
          this.atendimento.proximoContato.data = this.tempProximoContatoData;

          await atendimentoService.registrarParecerComProximoContato(
            atendimentoId,
            {
              parecer: parecerDescricao,
              proximoContato: {
                usuario: this.tempProximoContatoUsuario,
                data: this.tempProximoContatoData,
              },
            }
          );
          this.showToast(
            "Sucesso!",
            "Parecer registrado e próximo contato definido.",
            "success"
          );
        }
        this.cancelar();
        this.parecer = new Parecer();
        this.$refs.formParecer?.resetValidation?.();
        await this.carregarAtendimento();
      } catch (e) {
        console.error(e);
        this.showToast(
          "Erro",
          "Erro ao registrar o parecer. Verifique os dados e tente novamente.",
          "error"
        );
      } finally {
        this.isSubmitting = false;
      }
    },
    cancelar() {
      this.modalVisivel = false;
      this.parecerAcao = null;
      this.parecerEditando = null;
      this.estaEditandoParecer = false;
    },
    statusChipColor(status) {
      const colorsStatus = {
        Aberto: "var(--success)",
        Encerrado: "var(--error)",
      };
      return colorsStatus[status] || "grey";
    },
    abrirEdicaoParecer(parecer) {
      this.parecerEditando = { ...parecer };
      this.estaEditandoParecer = true;
      this.tituloModal = "Editar parecer";
      this.modalVisivel = true;
    },
    async salvarEdicaoParecer() {
      if (!this.parecerEditando.descricao) {
        return;
      }

      this.isSubmitting = true;
      try {
        await atendimentoService.editarParecer(
          this.atendimento.id,
          this.parecerEditando.id,
          {
            descricao: this.parecerEditando.descricao,
          },
          this.usuarioLogadoId
        );
        this.showToast("Sucesso!", "Parecer atualizado.", "success");

        this.cancelar();
        await this.carregarAtendimento();
      } catch (e) {
        console.error("Erro ao editar parecer:", e);
        this.showToast(
          "Erro",
          "Erro ao editar o parecer. Tente novamente.",
          "error"
        );
      } finally {
        this.isSubmitting = false;
      }
    },
  },
};
</script>

<style scoped>
.fill-height {
  height: 100vh;
}

.chip-status {
  width: 70px;
  padding: 0 10px;
  justify-content: center;
}

.status-wrapper {
  display: inline-flex;
}
</style>