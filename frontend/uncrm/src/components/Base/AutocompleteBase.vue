<template>
  <v-autocomplete
    :value="value"
    @input="$emit('input', $event)"
    :items="items"
    :label="label"
    :item-value="itemValue"
    :item-text="itemText"
    :multiple="multiple"
    :outlined="outlined"
    :dense="dense"
    :clearable="clearable"
    deletable-chips
    small-chips
    color="secondary"
    item-color="secondary"
  >
    <template v-slot:selection="{ item, index }">
      <v-chip
        v-if="index < maxVisibleChips"
        small
        class="ma-1"
        text-color="secondary"
        close
        outlined
        @click:close="removeItem(index)"
      >
        {{ getItemText(item) }}
      </v-chip>
      <span
        v-else-if="index === maxVisibleChips"
        class="ml-2 grey--text caption"
      >
        +{{ value.length - maxVisibleChips }}
      </span>
    </template>
  </v-autocomplete>
</template>

<script>
export default {
  name: "AutocompleteBase",
  props: {
    value: {
      type: Array,
      required: true,
    },
    items: {
      type: Array,
      required: true,
    },
    label: {
      type: String,
      default: "Selecione",
    },
    itemValue: {
      type: String,
      default: "id",
    },
    itemText: {
      type: String,
      default: "name",
    },
    multiple: {
      type: Boolean,
      default: true,
    },
    outlined: {
      type: Boolean,
      default: true,
    },
    dense: {
      type: Boolean,
      default: true,
    },
    clearable: {
      type: Boolean,
      default: true,
    },
    maxVisibleChips: {
      type: Number,
      default: 1,
    },
  },
  methods: {
    removeItem(index) {
      const copy = [...this.value];
      copy.splice(index, 1);
      this.$emit("input", copy);
    },
    getItemText(item) {
      return typeof item === "object" ? item[this.itemText] : item;
    },
  },
};
</script>
