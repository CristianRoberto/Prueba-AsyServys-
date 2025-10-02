<style scoped>


.formulario {
  max-width: 100%;
  margin: 20px auto;
  padding: 20px;
  background: #ffffff;
  border-radius: 10px;
  box-shadow: 0px 3px 6px rgba(0,0,0,0.1);
}

h2 {
  color: #0454b4;
  margin-bottom: 15px;
}

.encabezado {
  display: grid;
  grid-template-columns: repeat(2, 1fr);
  gap: 12px;
  margin-bottom: 20px;
}

.campo {
  display: flex;
  flex-direction: column;
}

.campo label {
  font-weight: 600;
  margin-bottom: 5px;
}

input {
  padding: 8px;
  border: 1px solid #ccc;
  border-radius: 6px;
  width: 100%;
}

h3 {
  margin: 20px 0 10px;
  color: #333;
}

/* Tabla */
.tabla-container {
  overflow-x: auto;
}

table {
  width: 100%;
  border-collapse: collapse;
}

thead th {
  background-color: #4c8cb4;
  color: white;
  text-align: center;
  padding: 10px;
}

tbody td {
  padding: 8px;
  text-align: center;
}

tbody input {
  width: 95%;
}

/* Botones */
button {
  cursor: pointer;
  border: none;
  border-radius: 6px;
  padding: 8px 14px;
  font-weight: 600;
  transition: 0.2s;
}

.btn-primario {
  background-color: #74ab3c;
  color: white;
}

.btn-primario:hover {
  background-color: #5f8f2e;
}

.btn-secundario {
  background-color: #0454b4;
  color: white;
  margin-top: 10px;
}

.btn-secundario:hover {
  background-color: #033f8c;
}

.btn-eliminar {
  background-color: #d9534f;
  color: white;
}

.btn-eliminar:hover {
  background-color: #b52b27;
}

.acciones {
  margin-top: 20px;
  text-align: right;
}

</style scoped>



<template>
  <div class="formulario">
    <h2>Registro de Control de Temperaturas</h2>

    <!-- Encabezado -->
    <div>
      <label>Destino:</label>
      <input v-model="form.destino" type="text" />

      <label>__Fecha Descongelación:</label>
      <input v-model="form.fechaDescongelacion" type="date" />

      <label>__Fecha Producción:</label>
      <input v-model="form.fechaProduccion" type="date" />

      <br>

      <label>__Realizado por:</label>
      <input v-model="form.realizadoPor" type="text" />

      <label>__Revisado por:</label>
      <input v-model="form.revisadoPor" type="text" />
    </div>

    <!-- Detalles -->
    <h3>Detalles</h3>
    <table>
      <thead>
        <tr>
          <th># Coche</th>
          <th>Código Producto</th>
          <th>Inicio Descongelado</th>
          <th>Temperatura</th>
          <th>Inicio Consumo</th>
          <th>Fin Consumo</th>
          <th>Observaciones</th>
          <th></th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="(d, i) in form.controlDetalles" :key="i">
          <td><input v-model="d.numeroCoche" type="number" /></td>
          <td><input v-model="d.codigoProducto" type="text" /></td>
          <td><input v-model="d.horaInicioDescongelado" type="time" /></td>
          <td><input v-model="d.temperaturaProducto" type="number" step="0.1" /></td>
          <td><input v-model="d.horaInicioConsumo" type="time" /></td>
          <td><input v-model="d.horaFinConsumo" type="time" /></td>
          <td><input v-model="d.observaciones" type="text" /></td>
          <td><button @click="eliminarDetalle(i)">X</button></td>
        </tr>
      </tbody>
    </table>
    <button @click="agregarDetalle">Agregar detalle</button>

    <!-- Botón Guardar -->
    <div>
      <button @click="guardar">Guardar</button>
    </div>
  </div>
</template>

<script setup lang="ts">
import { reactive } from 'vue'
import { useControlesStore } from '../stores/controles'

const store = useControlesStore()

const form = reactive({
  destino: '',
  fechaDescongelacion: '',
  fechaProduccion: '',
  realizadoPor: '',
  revisadoPor: '',
  controlDetalles: [] as any[]
})

const agregarDetalle = () => {
  form.controlDetalles.push({
    numeroCoche: 0,
    codigoProducto: '',
    horaInicioDescongelado: '',
    temperaturaProducto: 0,
    horaInicioConsumo: '',
    horaFinConsumo: '',
    observaciones: ''
  })
}

const eliminarDetalle = (index: number) => {
  form.controlDetalles.splice(index, 1)
}

const guardar = async () => {
  const payload = {
    destino: form.destino,
    fechaDescongelacion: form.fechaDescongelacion,
    fechaProduccion: form.fechaProduccion,
    realizadoPor: form.realizadoPor,
    revisadoPor: form.revisadoPor,
    controlDetalles: form.controlDetalles.map(d => ({
      numeroCoche: d.numeroCoche,
      codigoProducto: d.codigoProducto,
      horaInicioDescongelado: d.horaInicioDescongelado 
        ? d.horaInicioDescongelado + ':00' 
        : null,
      temperaturaProducto: d.temperaturaProducto,
      horaInicioConsumo: d.horaInicioConsumo 
        ? d.horaInicioConsumo + ':00' 
        : null,
      horaFinConsumo: d.horaFinConsumo 
        ? d.horaFinConsumo + ':00' 
        : null,
      observaciones: d.observaciones
    }))
  }

  try {
    await store.guardar(payload)
    alert('Guardado correctamente')

    // 🔹 Limpiar el formulario después de guardar
    form.destino = ''
    form.fechaDescongelacion = ''
    form.fechaProduccion = ''
    form.realizadoPor = ''
    form.revisadoPor = ''
    form.controlDetalles = [] // vacía la tabla de detalles
  } catch (e) {
    console.error(e)
    alert('Error al guardar, revisa consola')
  }
}








</script>
