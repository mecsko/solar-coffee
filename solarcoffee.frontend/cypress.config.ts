import { defineConfig } from "cypress";

export default defineConfig({
  component: {
    devServer: {
      framework: "vue",
      bundler: "vite",
    },
  },

  env: {
    ...process.env,
  },

  e2e: {
    supportFile: false,
    chromeWebSecurity: false,
    specPattern: "tests/cypress/**/*.ts",
  },
});
