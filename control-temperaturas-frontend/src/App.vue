<template>
  <div id="app">
    <nav class="navbar">
      <router-link to="/formulario">Formulario</router-link>
      <router-link to="/reporte">Reporte</router-link>
    </nav>
    <main>
      <router-view />
    </main>
  </div>
</template>

<script setup lang="ts">
// Nada aquí por ahora
</script>

<style>
body {
  font-family: Arial, sans-serif;
  margin: 0;
  padding: 0;
}

.navbar {
  display: flex;
  gap: 1rem;
  background: #4c8cb4;
  padding: 1rem;
}

.navbar a {
  color: white;
  text-decoration: none;
  font-weight: bold;
}

.navbar a.router-link-active {
  text-decoration: underline;
}

main {
  padding: 1rem;
}
</style>
