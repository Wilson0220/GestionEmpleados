import { createApp } from 'vue'
import App from './App.vue'
import router from './router'
import '@fortawesome/fontawesome-free/css/all.css'
import '@fortawesome/fontawesome-free/js/all.js'
import alertPlugin from './plugins/plugins/alertPlugin'

const app = createApp(App)
app.use(router)
app.use(alertPlugin)
app.mount('#app')
