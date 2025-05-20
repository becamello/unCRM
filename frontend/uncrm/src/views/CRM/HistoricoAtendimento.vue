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
              sm="12"
              class="d-flex align-center justify-start pb-1"
            >
              <breadcrumbs />
            </v-col>
          </v-row>
          <v-row class="mb-1">
            <v-col cols="12" md="3" class="py-0">
              <IconLabel :icon="icons.codigoAtendimento" label="Código">
                {{ atendimento.idFormatado }}
              </IconLabel>
              <IconLabel :icon="icons.pessoa" label="Pessoa">
                {{ atendimento.pessoaNome }}
              </IconLabel>
            </v-col>

            <v-col cols="12" md="3" class="py-0">
              <IconLabel :icon="icons.tipoAtendimento" label="Tipo atendimento">
                {{ atendimento.tipoAtendimentoDescricao }}
              </IconLabel>
              <IconLabel :icon="icons.status" label="Status">
                <div class="status-wrapper">
                  <v-chip
                    :color="statusChipColor(atendimento.statusItem)"
                    dark
                    x-small
                    outlined
                    class="chip-status"
                  >
                    {{ atendimento.statusItem }}
                  </v-chip>
                </div>
              </IconLabel>
            </v-col>

            <v-col cols="12" md="3" class="py-0">
              <IconLabel
                :icon="icons.usuarioGenerico"
                label="Usuário de abertura"
              >
                {{ atendimento.usuarioCriadorLogin }}
              </IconLabel>
              <IconLabel :icon="icons.dataGenerico" label="Data cadastro">
                {{ atendimento.dataCadastroFormatada }}
              </IconLabel>
            </v-col>

            <v-col cols="12" md="3" class="py-0">
              <IconLabel
                :icon="icons.usuarioGenerico"
                label="Usuário próximo contato"
              >
                {{ atendimento.proximoContato.usuarioLogin }}
              </IconLabel>
              <IconLabel
                :icon="icons.dataGenerico"
                label="Data próximo contato"
              >
                {{ atendimento.proximoContato.dataFormatada }}
              </IconLabel>
            </v-col>
          </v-row>
        </v-col>
      </v-row>
      <v-row>
        <v-col cols="12" md="12">
          <div style="max-height: 50vh; overflow-y: auto">
            <ParecerCard
              v-for="parecer in atendimento.pareceres"
              :key="parecer.id"
              :parecer="parecer"
              :icons="icons"
              :usuario-logado-id="usuarioLogadoId"
              :atendimento-encerrado="atendimentoEncerrado"
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
              :disabled="atendimentoEncerrado"
              label="Descrição do parecer"
              required
              outlined
              clearable
              rows="3"
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
        v-model="modalVisivel"
        @cancelar="cancelar"
        @salvar="confirmarAcaoParecer"
        :titulo-modal="tituloModal"
        width="50%"
      >
        <v-overlay v-if="isSubmitting" absolute>
          <v-progress-circular indeterminate color="primary" size="30" />
        </v-overlay>
        <v-form ref="formModal" lazy-validation>
          <v-radio-group v-model="parecerAcao" class="mb-4">
            <v-radio label="Encerrar atendimento" value="encerrar"></v-radio>
            <v-radio label="Definir próximo contato" value="proximo"></v-radio>
          </v-radio-group>

          <div v-if="parecerAcao === 'proximo'">
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
          </div>
        </v-form>
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
      tituloModal: "Próximo passo:",
      parecerAcao: null,
      proximoContatoData: "",
      proximoContatoUsuario: "",
      isSubmitting: false,
      modalVisivel: false,
      usuarios: [],
      parecer: new Parecer(),
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
        this.atendimento = new Atendimento(response.data);
        console.log(response.data);
        console.log("Pareceres recebidos:", response.data.pareceres);
      } catch (error) {
        console.error("Erro ao carregar atendimento:", error);
      }
    },
    abrirModal() {
      if (this.$refs.formParecer.validate()) {
        this.modalVisivel = true;
        this.atendimento.proximoContato.data = null;
        this.atendimento.proximoContato.usuario = null;
      }
    },
    async confirmarAcaoParecer() {
      if (!this.parecerAcao) return;

      const parecerDescricao = this.parecer.parecer || this.parecer;
      const atendimentoId = this.atendimento.id;

      this.isSubmitting = true;

      try {
        if (this.parecerAcao === "encerrar") {
          await atendimentoService.registrarParecerComEncerramento(
            atendimentoId,
            { parecer: parecerDescricao }
          );
        }

        if (this.parecerAcao === "proximo") {
          if (
            !this.atendimento.proximoContato?.usuario ||
            !this.atendimento.proximoContato?.data
          ) {
            return;
          }

          await atendimentoService.registrarParecerComProximoContato(
            atendimentoId,
            {
              parecer: parecerDescricao,
              proximoContato: {
                usuario: this.atendimento.proximoContato.usuario,
                data: this.atendimento.proximoContato.data,
              },
            }
          );
        }
        this.cancelar();
        this.parecer = new Parecer();
        this.$refs.formParecer?.resetValidation?.();
        await this.carregarAtendimento();
      } catch (e) {
        console.error(e);
      } finally {
        this.isSubmitting = false;
      }
    },
    cancelar() {
      this.modalVisivel = false;
      this.parecerAcao = null;
    },
    statusChipColor(status) {
      const colorsStatus = {
        Aberto: "var(--success)",
        Encerrado: "var(--error)",
      };
      return colorsStatus[status] || "grey";
    },
  },
};
</script>

<style scoped>
.parecer {
  background-color: var(--background-parecer);
}
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