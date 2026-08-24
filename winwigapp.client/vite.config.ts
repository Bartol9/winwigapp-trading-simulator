import { defineConfig } from 'vite'
import path from 'path'
import tailwindcss from '@tailwindcss/vite'
import react from '@vitejs/plugin-react'

export default defineConfig({
    plugins: [
        react(),
        tailwindcss(),
    ],
    resolve: {
        alias: {
            '@': path.resolve(__dirname, './src'),
        },
    },
    server: {
        port: 5173, // Standardowy port Vite
        proxy: {
            // Przekierowuje zapytania /api na port Twojego backendu ASP.NET
            '^/api': {
                target: 'https://localhost:7054',
                secure: false
            }
        }
    },
    build: {
        chunkSizeWarningLimit: 600,
        rollupOptions: {
            output: {
                manualChunks: {
                    react: ['react', 'react-dom', 'react-router'],
                    charts: ['recharts'],
                    icons: ['lucide-react'],
                },
            },
        },
    },
    assetsInclude: ['**/*.svg', '**/*.csv'],
})
