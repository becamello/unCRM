<template>
  <v-dialog v-model="dialogInterno" max-width="50%" persistent >
    <v-card>
      <v-card-title class="headline" style="color: var(--text-primary);">
        {{ tituloModal }}
      </v-card-title>

      <v-card-text class="pa-3">
        <slot />
      </v-card-text>

      <v-card-actions class="d-flex justify-end pa-6">
        <BotaoBase variante="secundario" @click="$emit('cancelar')" width="20%">Cancelar</BotaoBase>
        <BotaoBase variante="primario" @click="$emit('salvar')" width="20%">Salvar</BotaoBase>
      </v-card-actions>
    </v-card>
  </v-dialog>
</template>

<script>
import BotaoBase from './BotaoBase.vue';

export default {
  name: "ModalFormulario",
  components: { BotaoBase },
  props: {
    value: Boolean, 
    tituloModal: {
      type: String    },
  },
  computed: {
    dialogInterno: {
      get() {
        return this.value;
      },
      set(val) {
        this.$emit('input', val);
      }
    }
  }
};
</script>
