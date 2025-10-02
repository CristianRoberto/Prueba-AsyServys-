import { defineStore } from 'pinia'
import axios from '../services/http'

export const useControlesStore = defineStore('controles', {
  state: () => ({
    lista: [] as any[],
    reporte: [] as any[]
  }),
  actions: {
    async listar() {
      const resp = await axios.get('/ControlEncabezados/Lista')
      this.lista = resp.data
    },
    async buscar(id: number) {
      return await axios.get(`/ControlEncabezados/Buscar/${id}`)
    },
    async guardar(data: any) {
      return await axios.post('/ControlEncabezados/Guardar', data)
    },
    async editar(data: any) {
      return await axios.put('/ControlEncabezados/Editar', data)
    },
    async eliminar(id: number) {
      return await axios.delete(`/ControlEncabezados/Eliminar/${id}`)
    },
    async obtenerReporte(desde: string, hasta: string, destino?: string) {
      const params: any = { desde, hasta }
      if (destino) params.destino = destino
      const resp = await axios.get('/ControlEncabezados/Reporte', { params })
      this.reporte = resp.data
    }
  }
})
