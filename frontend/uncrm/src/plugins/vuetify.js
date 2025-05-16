import Vue from "vue";
import Vuetify from "vuetify/lib/framework";
import { lightTheme, darkTheme, themeVars } from '@/constants/colors';

const isDark = localStorage.getItem('darkMode') === 'true';
themeVars(isDark ? darkTheme : lightTheme);

Vue.use(Vuetify);

export default new Vuetify({
  icons: {
    iconfont: "mdi",
  },
  lang: {
    locales: { "pt-br": require("vuetify/es5/locale/pt").default },
    current: "pt-br",
  },
  theme: {
    options: { customProperties: true },
    themes: {
      light: lightTheme,
      dark: darkTheme,
    },
  },
});
