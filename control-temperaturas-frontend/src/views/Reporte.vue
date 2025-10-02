<template>
  <div class="reporte">
    <h2>Reporte de Controles</h2>

    <div>
      <label>Desde:</label>
      <input v-model="filtros.desde" type="date" />

      <label>Hasta:</label>
      <input v-model="filtros.hasta" type="date" />

      <label>Destino:</label>
      <input v-model="filtros.destino" type="text" />

      <button @click="buscarReporte">Buscar</button>
    </div>

    <table v-if="store.reporte.length > 0">
      <thead>
        <tr>
          <th>Destino</th>
          <th>Fecha Producción</th>
          <th># Coche</th>
          <th>Codigo Producto</th>
          <th>Temp</th>
          <th>Inicio Consumo</th>
          <th>Fin Consumo</th>
          <th>Observacion</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="(r, i) in store.reporte" :key="i">
          <td>{{ r.destino }}</td>
          <td>{{ r.fechaProduccion }}</td>
          <td>{{ r.numeroCoche }}</td>
          <td>{{ r.codigoProducto }}</td>
          <td>{{ r.temperaturaProducto }}</td>
          <td>{{ r.horaInicioConsumo }}</td>
          <td>{{ r.horaFinConsumo }}</td>
          <td>{{ r.observaciones }}</td>
        </tr>
      </tbody>
    </table>
  </div>
</template>

<script setup lang="ts">
import { reactive } from 'vue'
import { useControlesStore } from '../stores/controles'

const store = useControlesStore()

const filtros = reactive({
  desde: '',
  hasta: '',
  destino: ''
})

const buscarReporte = async () => {
  await store.obtenerReporte(filtros.desde, filtros.hasta, filtros.destino)
}
</script>
