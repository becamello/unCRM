<template>
  <div class="toast-container">
    <ToastItem
      v-for="toast in toasts"
      :key="toast.id"
      :title="toast.title"
      :message="toast.message"
      :type="toast.type"
      :timeout="toast.timeout"
      @close="removeToast(toast.id)"
    />
  </div>
</template>

<script>
import ToastItem from "./ToastItem.vue";

export default {
  name: "ToastContainer",
  components: { ToastItem },
  data() {
    return {
      toasts: [],
    };
  },
  
  methods: {
    addToast(title, message, type = "info", timeout = 3000) {
      const id = Date.now() + Math.random();
      this.toasts.push({ id, title, message, type, timeout });
    },

    removeToast(id) {
      this.toasts = this.toasts.filter((t) => t.id !== id);
    },
  },
};
</script>

<style scoped>
.toast-container {
  position: fixed;
  top: 20px;
  right: 20px;
  z-index: 9999;
  display: flex;
  flex-direction: column;
  align-items: flex-end;
  gap: 8px;
}
</style>
