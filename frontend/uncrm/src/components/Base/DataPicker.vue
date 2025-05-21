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
        v-model="formattedDate"
        :label="label"
        prepend-inner-icon="mdi-calendar"
        color="secondary"
        outlined
        clearable
        :dense="dense"
        v-bind="attrs"
        v-on="on"
        :rules="rules"
      />
    </template>

    <v-date-picker
      v-model="internalDate"
      locale="pt-br"
      no-title
      :show-current="false"
      color="var(--text-primary)"
      :min="min"
      :max="max"
      :allowed-dates="allowedDates"
      @input="onSelectDate"
    />
  </v-menu>
</template>

<script>
export default {
  inheritAttrs: false,
  props: {
    value: String,
    label: { type: String, default: "Data" },
    rules: { type: Array, default: () => [] },
    dense: { type: Boolean, default: false },
    min: String,
    max: String,
    allowedDates: Function,
  },
  data() {
    return {
      menu: false,
      internalDate: this.value || null,
    };
  },
  computed: {
    formattedDate: {
      get() {
        if (!this.internalDate) return "";
        const [year, month, day] = this.internalDate.split("-");
        return `${day}/${month}/${year}`;
      },
      set(val) {
        if (!val) {
          this.updateDate(null);
          return;
        }
        const [day, month, year] = val.split("/");
        this.updateDate(`${year}-${month}-${day}`);
      },
    },
  },
  watch: {
    value(val) {
      this.internalDate = val;
    },
  },
  methods: {
    updateDate(date) {
      this.internalDate = date;
      this.$emit("input", date);
    },
    onSelectDate(date) {
      this.menu = false;
      this.updateDate(date);
    },
  },
};
</script>

<style scoped>
::v-deep .v-btn.v-btn--active.v-btn--rounded,
::v-deep .v-btn.v-size--default.v-btn--active {
  background-color: var(--primary) !important;
}
</style>
