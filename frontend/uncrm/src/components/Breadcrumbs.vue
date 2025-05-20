<template>
  <v-breadcrumbs divider="/" class="pa-0 py-2">
    <v-breadcrumbs-item
      v-for="(crumb, index) in breadcrumbs"
      :key="index"
      :to="crumb.to"
      :disabled="index === breadcrumbs.length - 1"
      link
    >
      {{ crumb.text }}
    </v-breadcrumbs-item>
  </v-breadcrumbs>
</template>

<script>
export default {
  name: "BreadcrumbsTitle",
  computed: {
    breadcrumbs() {
      return this.$route.matched.map(route => ({
        text: route.meta.title || route.name,
        to: { path: route.path }
      }));
    },
  }
};
</script>
  
  <style>
.v-breadcrumbs__item {
  font-size: 28px;
  color: var(--text-primary) !important;;
  font-weight: bold;
}
  </style>