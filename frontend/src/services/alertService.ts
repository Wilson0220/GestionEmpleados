import Swal, { SweetAlertResult } from 'sweetalert2'

export interface AlertService {
  confirmar: (title?: string, text?: string, confirmText?: string, cancelText?: string) => Promise<boolean>
  exito: (title?: string, text?: string) => void
  error: (title?: string, text?: string) => void
  advertencia: (title?: string, text?: string) => void
  info: (title?: string, text?: string) => void
  pregunta: (title?: string, text?: string) => Promise<SweetAlertResult>
  toast: (title?: string, icon?: 'success' | 'error' | 'info' | 'warning' | 'question') => void
}

export const useAlert = (): AlertService => {
  const confirmar = async (
    title = '¿Estás seguro?',
    text = 'Esta acción no se puede deshacer',
    confirmText = 'Sí, continuar',
    cancelText = 'Cancelar'
  ): Promise<boolean> => {
    const res = await Swal.fire({
      title,
      text,
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: confirmText,
      cancelButtonText: cancelText,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33'
    })
    return res.isConfirmed
  }

  const exito = (title = 'Éxito', text = 'Operación realizada correctamente') => {
    Swal.fire({ title, text, icon: 'success', confirmButtonText: 'Aceptar' })
  }

  const error = (title = 'Error', text = 'Ocurrió un error inesperado') => {
    Swal.fire({ title, text, icon: 'error', confirmButtonText: 'Aceptar' })
  }

  const advertencia = (title = 'Advertencia', text = 'Verifica esta información') => {
    Swal.fire({ title, text, icon: 'warning', confirmButtonText: 'Entendido' })
  }

  const info = (title = 'Información', text = '') => {
    Swal.fire({ title, text, icon: 'info', confirmButtonText: 'Ok' })
  }

  const pregunta = async (
    title = '¿Deseas continuar?',
    text = 'Confirma para proceder'
  ): Promise<SweetAlertResult> => {
    return await Swal.fire({
      title,
      text,
      icon: 'question',
      showCancelButton: true,
      confirmButtonText: 'Sí',
      cancelButtonText: 'No'
    })
  }

  const toast = (title = 'Operación completada', icon: 'success' | 'error' | 'info' | 'warning' | 'question' = 'success') => {
    Swal.fire({
      toast: true,
      position: 'top-end',
      icon,
      title,
      showConfirmButton: false,
      timer: 3000,
      timerProgressBar: true,
      background: '#fff',
      color: '#333',
      iconColor: icon === 'success' ? '#28a745' : icon === 'error' ? '#dc3545' : '#007bff'
    })
  }

  return { confirmar, exito, error, advertencia, info, pregunta, toast }
}
