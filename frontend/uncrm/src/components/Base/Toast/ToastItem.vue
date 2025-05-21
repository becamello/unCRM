<template>
  <v-snackbar
    v-model="isVisible"
    :timeout="timeout"
    top
    right
    outlined
    :color="colorName"
    class="app-toast"
    multi-line
  >
    <div class="toast-container">
      <v-icon class="toast-icon" :style="{ color: computedColor }">
        {{ icon }}
      </v-icon>
      <div class="toast-content">
        <div class="toast-title" :style="{ color: computedColor }">
          {{ title }}
        </div>
        <div class="toast-subtitle">
          {{ message }}
        </div>
      </div>
    </div>

    <template v-slot:action>
      <v-btn icon @click="close">
        <v-icon>mdi-close</v-icon>
      </v-btn>
    </template>
  </v-snackbar>
</template>

<script>
import { toastIcons } from "@/constants/icons"; 

export default {
  props: {
    title: String,
    message: String,
    type: {
      type: String,
      default: "info",
    },
    timeout: {
      type: Number,
      default: 2000,
    },
  },
  data() {
    return {
      isVisible: true,
    };
  },
  computed: {
    icon() {
      return toastIcons[this.type] || toastIcons.info;
    },
    colorName() {
      return this.type || "info";
    },
    computedColor() {
      const cssVars = {
        success: "var(--success)",
        error: "var(--error)",
        warning: "var(--warning)",
        info: "var(--info)",
      };
      return cssVars[this.type] || cssVars.info;
    },
  },
  methods: {
    close() {
      this.isVisible = false;
      this.$emit("close");
    },
  },
};
</script>

<style scoped>
.toast-container {
  display: flex;
  align-items: flex-start;
  gap: 12px;
  max-width: 400px;
}

.toast-icon {
  font-size: 28px;
  margin-top: 2px;
}

.toast-content {
  display: flex;
  flex-direction: column;
}

.toast-title {
  font-weight: bold;
  font-size: 16px;
}

.toast-subtitle {
  color: var(--text-primary);
  font-size: 14px;
}
</style>
