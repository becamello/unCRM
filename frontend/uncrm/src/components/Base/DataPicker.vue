<template>
  <v-menu
    v-model="menu"
    :close-on-content-click="false"
    offset-y
    nudge-bottom="0"
    max-width="290px"
  >
    <template #activator="{ on, attrs }">
      <v-text-field
        color="secondary"
        v-model="formattedDate"
        :label="label"
        prepend-inner-icon="mdi-calendar"
        v-bind="attrs"
        v-on="on"
        outlined
        clearable
        dense
        @click:clear="clearDate"
      ></v-text-field>
    </template>
    <v-date-picker
      v-model="internalDate"
      locale="pt-br"
      @input="selectDate"
      no-title
      :show-current="false"
      color="var(--text-primary)"
    ></v-date-picker>
  </v-menu>
</template>

<script>
export default {
  props: {
    value: String,
    label: {
      type: String,
      default: 'Data de Cadastro',
    },
  },
  data() {
    return {
      menu: false,
      internalDate: this.value,  
    };
  },
  computed: {
    formattedDate: {
      get() {
        if (!this.internalDate) return ''
        const [year, month, day] = this.internalDate.split('-')
        return `${day}/${month}/${year}`
      },
      set(newValue) {
        const [month, year, day] = newValue.split('-');
        this.internalDate = `${day}/${month}/${year}`; 
        this.$emit('input', this.internalDate); 
      },
    },
  },
  watch: {
    value(newValue) {
      this.internalDate = newValue;
    },
  },
  methods: {
    selectDate(date) {
      this.menu = false;
      this.$emit('input', date); 
    },
    clearDate() {
      this.internalDate = null;
      this.$emit('input', null); 
    },
  },
};
</script>

<style scoped>
::v-deep .v-btn.v-btn--active.v-btn--rounded, ::v-deep .v-btn.v-size--default.v-btn--active{
    background-color: var(--primary)!important;
}
</style>
