import { App } from 'vue'
import { useAlert, AlertService } from '@/services/alertService'

declare module '@vue/runtime-core' {
  interface ComponentCustomProperties {
    $alert: AlertService
  }
}

export default {
  install(app: App) {
    const alert = useAlert()
    app.config.globalProperties.$alert = alert
  }
}
