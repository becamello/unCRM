<template>
  <div class="d-flex align-center mb-1 label-info">
    <v-icon small class="mr-2">{{ icon }}</v-icon>
    <span class="d-flex align-center"
      ><strong class="mr-2">{{ label }}: </strong> <slot
    /></span>
  </div>
</template>

<script>
export default {
  props: {
    icon: { type: String, required: true },
    label: { type: String, required: true },
  },
};
</script>

<style>
.label-info {
  font-size: 14px;
}
</style>
