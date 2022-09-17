import { defineConfig } from "vitest/config";
import vue from "@vitejs/plugin-vue";
import EnvironmentPlugin from "vite-plugin-environment";

// https://vitejs.dev/config/
export default defineConfig({
  server: {
    port: 8080,
    proxy: { "/api": { target: "https://localhost:5001/api", secure: false } },
  },
  plugins: [vue(), EnvironmentPlugin("all")],
  resolve: {
    alias: {
      "@": "/src/",
    },
  },
  test: {
    include: ["tests/**/**/*.{test,spec,cy}.{js,jsx,ts,tsx}"],
    globals: true,
    environment: "jsdom",
  },
});
