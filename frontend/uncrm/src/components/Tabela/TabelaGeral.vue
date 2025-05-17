<template>
  <v-data-table
    :headers="headers"
    :items="items"
    :items-per-page="-1"
    class="elevation-0 custom-scroll"
    no-title
    fixed-header
    :hide-default-footer="hideFooter"
    v-bind="$attrs"
  >
    <template v-slot:[`item.acoes`]="{ item }">
      <v-menu offset-y>
        <template v-slot:activator="{ on, attrs }">
          <v-btn icon v-bind="attrs" v-on="on">
            <v-icon small>{{ icons.subMenu }}</v-icon>
          </v-btn>
        </template>
        <v-list dense>
          <v-list-item
            v-for="(acao, index) in actions"
            :key="index"
            @click="acao.handler(item)"
            :disabled="acao.disabled && acao.disabled(item)"
          >
            <v-list-item-icon class="mr-2">
              <v-icon small class="icon-table">
                {{ acao.icon }}
              </v-icon>
            </v-list-item-icon>
            <v-list-item-content>
              <v-list-item-title>{{ acao.label }}</v-list-item-title>
            </v-list-item-content>
          </v-list-item>
        </v-list>
      </v-menu>
    </template>

    <template v-slot:[`item.statusItem`]="{ item }">
      <v-chip
        :color="statusChipColor(item.statusItem)"
        dark
        small
        outlined
        class="chip-status"
      >
        {{ item.statusItem }}
      </v-chip>
    </template>
  </v-data-table>
</template>

<script>
import { icons } from "@/constants/icons";

export default {
  name: "TabelaGeral",
  data() {
    return {
      icons,
    };
  },
  props: {
    headers: {
      type: Array,
      required: true,
    },
    items: {
      type: Array,
      required: true,
    },
    actions: {
      type: Array,
      default: () => [],
    }
  },
  methods: {
    statusChipColor(status) {
      const colorsStatus = {
        Aberto: "var(--success)",
        Encerrado: "var(--error)",
        Ativo: "var(--success)",
        Inativo: "var(--error)",
      };
      return colorsStatus[status] || "grey";
    },
  },
};
</script>

<style scoped>
.icon-table {
  cursor: pointer;
}

.chip-status {
  width: 90px;
  display: flex;
  justify-content: center;
}
</style>
