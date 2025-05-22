<template>
  <v-dialog v-model="dialogInterno" v-bind="$attrs" persistent :fullscreen="isMobile">
    <v-card class="pa-2">
      <v-card-title class="headline pa-4 mb-4" style="color: var(--text-primary);">
        {{ tituloModal }}
      </v-card-title>

      <v-card-text class="px-4 pa-0">
        <slot />
      </v-card-text>

      <v-card-actions class="d-flex justify-end pa-2 pt-4">
        <BotaoBase variante="secundario" @click="$emit('cancelar')" width="24%">Cancelar</BotaoBase>
        <BotaoBase variante="primario" @click="$emit('salvar')" width="24%">Salvar</BotaoBase>
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
    },
    isMobile() {
      if (this.forceFullscreen) return true;
      return window.innerWidth <= 600;
    }
  },
  mounted() {
    window.addEventListener('resize', this.onResize);
  },
  beforeDestroy() {
    window.removeEventListener('resize', this.onResize);
  },
  methods: {
    onResize() {
      this.$forceUpdate();
    }
  }
};
</script>
