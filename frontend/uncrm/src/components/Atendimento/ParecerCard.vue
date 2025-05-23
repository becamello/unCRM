<template>
  <v-card class="mb-3 parecer" elevation="0">
    <v-card-text>
      <v-card-actions
        class="d-flex align-center justify-space-between pa-0 pb-1"
      >
        <p class="ma-0" style="font-size: 12px">{{ parecer.usuarioCriadorLogin }} - {{ parecer.dataFormatada }}</p>
        <v-tooltip bottom>
          <template v-slot:activator="{ on, attrs }">
            <span v-bind="attrs" v-on="on">
              <v-btn
                icon
                v-if="
                  Number(parecer.usuarioCriadorId) === Number(usuarioLogadoId)
                "
                :disabled="atendimentoEncerrado"
                @click="$emit('editar', parecer)"
              >
                <v-icon dense>{{ icons.editar }}</v-icon>
              </v-btn>
            </span>
          </template>
          <span v-if="atendimentoEncerrado">
            Atendimentos encerrados não podem editar pareceres
          </span>
          <span v-else> Editar parecer </span>
        </v-tooltip>
      </v-card-actions>
      <div class="text--primary">
        {{ parecer.descricao }}
      </div>
    </v-card-text>
  </v-card>
</template>

<script>
export default {
  props: {
    parecer: Object,
    icons: Object,
    usuarioLogadoId: Number,
    atendimentoEncerrado: Boolean,
  },
};
</script>

<style>
.parecer {
  background-color: var(--background-parecer)!important;
}
</style>