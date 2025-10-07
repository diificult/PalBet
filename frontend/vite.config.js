import { defineConfig } from "vite";
import react from "@vitejs/plugin-react";
import tailwindcss from "@tailwindcss/vite";
import markdown, { Mode } from "vite-plugin-markdown";
// https://vite.dev/config/
export default defineConfig({
    plugins: [react(), tailwindcss()],
});
