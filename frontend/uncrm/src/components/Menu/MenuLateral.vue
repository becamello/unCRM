<template>
  <v-navigation-drawer
    v-model="drawer"
    :mini-variant="mini"
    permanent
    dark
    app
    class="nav-bar"
    width="16vw"
  >
    <v-btn icon @click.stop="toggleMini" class="ma-3">
      <v-icon>{{ icons.menu }}</v-icon>
    </v-btn>
    <div class="nav-container">
      <v-list>
        <v-list-item
          v-for="item in menuItems"
          :key="item.to"
          router
          :to="item.to"
          @click="closeDrawer"
        >
          <v-list-item-action class="mr-2">
            <v-icon>{{ icons[item.icon] }}</v-icon>
          </v-list-item-action>
          <v-list-item-content>
            <v-list-item-title class="nav-title">{{
              item.title
            }}</v-list-item-title>
          </v-list-item-content>
        </v-list-item>
      </v-list>
      <v-list>
        <v-list-item
          v-for="item in footerItems"
          :key="item.title"
          @click="handleFooterAction(item.action)"
        >
          <v-list-item-action class="mr-2">
            <v-icon>{{ icons[item.icon] }}</v-icon>
          </v-list-item-action>
          <v-list-item-content>
            <v-list-item-title class="nav-title">{{
              item.title
            }}</v-list-item-title>
          </v-list-item-content>
        </v-list-item>
      </v-list>
    </div>
  </v-navigation-drawer>
</template>

<script>
import { icons } from "@/constants/icons";

export default {
  name: "MenuLateral",
  data() {
    return {
      icons,
      drawer: true,
      mini: true,
      menuItems: [
        { title: "Página inicial", icon: "paginaInicial", to: "/" },
        { title: "Atendimentos", icon: "atendimento", to: "/crm-atendimentos" },
        { title: "Cadastro de usuário", icon: "usuario", to: "/usuarios" },
        { title: "Cadastro de pessoa", icon: "pessoa", to: "/pessoas" },
      ],
      footerItems: [
        { title: "Alternar tema", icon: "alternarTema", action: "toggleTheme" },
        { title: "Deslogar", icon: "logout", action: "logout" },
      ],
    };
  },
  created() {
    const darkMode = localStorage.getItem("darkMode");
    if (darkMode !== null) {
      this.$vuetify.theme.dark = darkMode === "true";
    }
  },
  methods: {
    toggleMini() {
      this.mini = !this.mini;
    },
    closeDrawer() {
      this.mini = true;
    },
    toggleTheme() {
      this.$vuetify.theme.dark = !this.$vuetify.theme.dark;
      localStorage.setItem("darkMode", this.$vuetify.theme.dark);
    },
    logout() {
      console.log("Logout realizado");
    },
    handleFooterAction(action) {
      if (typeof this[action] === "function") {
        this[action]();
      }
    },
  },
};
</script>
<style scoped>
.nav-bar {
  background-color: var(--primary-color);
}

.nav-title {
  margin-left: 8px;
  text-align: left;
  font-size: 14px;
}

.nav-container {
  height: 90vh;
  display: flex;
  flex-direction: column;
  justify-content: space-between;
}

@media (max-width: 600px) {
  .nav-bar {
    width: auto !important;
  }
}
</style>
