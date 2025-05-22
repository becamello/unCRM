import Vue from 'vue'
import App from './App.vue'
import router from './router'
import vuetify from './plugins/vuetify'
import Highcharts from 'highcharts';
import '@mdi/font/css/materialdesignicons.css'
import '@/assets/styles/colors.css'
import { chartColors  } from './constants/colors'

Highcharts.setOptions({
  colors: chartColors
});

Vue.config.productionTip = false

new Vue({
  router,
  vuetify,
  render: h => h(App)
}).$mount('#app')

