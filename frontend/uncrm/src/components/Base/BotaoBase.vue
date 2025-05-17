<template>
  <v-btn
    :color="color"
    :outlined="isSecondary"
    v-bind="$attrs"
    v-on="$listeners"
  >
    <v-icon v-if="iconeBtn" left>{{ iconeBtn }}</v-icon>
    <slot />
  </v-btn>
</template>

<script>
export default {
  name: "AppButton",
  props: {
    variante: {
      type: String,
      default: "primario",
    },
     iconeBtn: {
      type: String,
      default: null,
    },
  },
  computed: {
    isSecondary() {
      return this.variante === "secundario";
    },
    color() {
      return this.variante === 'primario' ? 'primary' : 'secondary'
    }
  },
};
</script>
