const lightTheme = {
  primary: "#534194",
  secondary: "#252525",
  success: "#2ECC71",
  error: "#E74C3C",
  warning: "#F1C40F",
  info: "#3498DB",

  "text-primary": "#534194",
  "background-parecer": '#f0f0f0'
};

const darkTheme = {
  primary: "#534194",
  secondary: "#F9F9F9",
  success: "#27AE60",
  error: "#E57373",
  warning: "#FDD835",
  info: "#64B5F6",

  "text-primary": "#fff",
  "background-parecer": '#1e1e1e'
};

function themeVars(theme) {
  const root = document.documentElement;
  Object.entries(theme).forEach(([key, value]) => {
    root.style.setProperty(`--${key}`, value);
  });
}

export { lightTheme, darkTheme, themeVars };
