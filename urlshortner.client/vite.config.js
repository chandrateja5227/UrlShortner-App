import { fileURLToPath, URL } from 'node:url'
import { defineConfig } from 'vite'
import vue from '@vitejs/plugin-vue'

export default defineConfig({
  plugins: [vue()],
  resolve: {
    alias: {
      '@': fileURLToPath(new URL('./src', import.meta.url))
    }
  },
  server: {
    proxy: {
      '^/st': {
        target: process.env.VITE_API_URL || 'http://localhost:7224',
        changeOrigin: true,
        secure: false
      }
    },
    port: 2510,
    host: true
  },
  build: {
    outDir: 'dist',
    assetsDir: 'assets'
  }
})
