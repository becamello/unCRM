 <template>
  <div class="table-wrapper">
    <v-data-table
      :headers="headers"
      :items="items"
      :items-per-page="-1"
      class="elevation-0 custom-scroll"
      no-title
      fixed-header
      v-bind="$attrs"
    >
      <template v-slot:[`item.nome`]="{ item }">
        <div
          ref="nomeCells"
          class="truncate-cell"
          :style="{ maxWidth: getHeaderWidth('nome') }"
          @mouseenter="checkTruncate($event, 'nome', item)"
          @mouseleave="clearTruncate('nome', item)"
          :aria-label="item.nome"
        >
          <v-tooltip v-if="isTruncated('nome', item)" bottom>
            <template #activator="{ on, attrs }">
              <span v-bind="attrs" v-on="on">{{ item.nome }}</span>
            </template>
            <span>{{ item.nome }}</span>
          </v-tooltip>
          <template v-else>
            {{ item.nome }}
          </template>
        </div>
      </template>

      <template v-slot:[`item.nomeCurto`]="{ item }">
        <div
          ref="nomeCurtoCells"
          class="truncate-cell"
          :style="{ maxWidth: getHeaderWidth('nomeCurto') }"
          @mouseenter="checkTruncate($event, 'nomeCurto', item)"
          @mouseleave="clearTruncate('nomeCurto', item)"
          :aria-label="item.nomeCurto"
        >
          <v-tooltip v-if="isTruncated('nomeCurto', item)" bottom>
            <template #activator="{ on, attrs }">
              <span v-bind="attrs" v-on="on">{{ item.nomeCurto }}</span>
            </template>
            <span>{{ item.nomeCurto }}</span>
          </v-tooltip>
          <template v-else>
            {{ item.nomeCurto }}
          </template>
        </div>
      </template>

      <template v-slot:[`item.login`]="{ item }">
        <div
          ref="loginCells"
          class="truncate-cell"
          :style="{ maxWidth: getHeaderWidth('login') }"
          @mouseenter="checkTruncate($event, 'login', item)"
          @mouseleave="clearTruncate('login', item)"
          :aria-label="item.login"
        >
          <v-tooltip v-if="isTruncated('login', item)" bottom>
            <template #activator="{ on, attrs }">
              <span v-bind="attrs" v-on="on">{{ item.login }}</span>
            </template>
            <span>{{ item.login }}</span>
          </v-tooltip>
          <template v-else>
            {{ item.login }}
          </template>
        </div>
      </template>
      <template v-slot:[`item.pessoaNome`]="{ item }">
        <div
          ref="pessoaNomeCells"
          class="truncate-cell"
          :style="{ maxWidth: getHeaderWidth('pessoaNome') }"
          @mouseenter="checkTruncate($event, 'pessoaNome', item)"
          @mouseleave="clearTruncate('pessoaNome', item)"
          :aria-label="item.pessoaNome"
        >
          <v-tooltip v-if="isTruncated('pessoaNome', item)" bottom>
            <template #activator="{ on, attrs }">
              <span v-bind="attrs" v-on="on">{{ item.pessoaNome }}</span>
            </template>
            <span>{{ item.pessoaNome }}</span>
          </v-tooltip>
          <template v-else>
            {{ item.pessoaNome }}
          </template>
        </div>
      </template>

      <template v-slot:[`item.acoes`]="slotProps">
        <div class="d-flex align-center">
          <slot name="botoesExtras" v-bind="slotProps" />
          <v-menu offset-y>
            <template v-slot:activator="{ on, attrs }">
              <v-btn icon v-bind="attrs" v-on="on">
                <v-icon small>{{ actionIcon }}</v-icon>
              </v-btn>
            </template>
            <v-list dense>
              <v-list-item
                v-for="(acao, index) in actions"
                :key="index"
                @click="acao.handler && acao.handler(slotProps.item)"
                :disabled="acao.disabled && acao.disabled(slotProps.item)"
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
        </div>
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
  </div>
</template>

<script>
import { icons } from "@/constants/icons";

export default {
  name: "TabelaGeral",
  props: {
    headers: { type: Array, required: true },
    items: { type: Array, required: true },
    actions: { type: Array, default: () => [] },
    actionIcon: { type: String, default: icons.subMenu },
  },
  data() {
    return {
      truncatedCells: {},
    };
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

    getHeaderWidth(col) {
      const header = this.headers.find((h) => h.value === col);
      return header?.width || "150px";
    },

    checkTruncate(event, col, item) {
      const el = event.currentTarget;
      const isTruncated = el.scrollWidth > el.clientWidth;
      const key = `${col}-${item.id}`;
      this.$set(this.truncatedCells, key, isTruncated);
    },

    clearTruncate(col, item) {
      const key = `${col}-${item.id}`;
      this.$set(this.truncatedCells, key, false);
    },

    isTruncated(col, item) {
      const key = `${col}-${item.id}`;
      return !!this.truncatedCells[key];
    },
  },
};
</script>

<style scoped>
.table-wrapper {
  overflow-x: auto;
  -webkit-overflow-scrolling: touch;
  width: 100%;
}

.truncate-cell {
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
  cursor: default;
}

.chip-status {
  width: 80px;
  display: flex;
  justify-content: center;
}
</style>