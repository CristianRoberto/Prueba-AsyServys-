# Control de Temperaturas - Prueba Técnica
Este proyecto corresponde a la digitalización del **formato de Control de Temperaturas de Producto Destinado a Conservas**, solicitado como parte de la prueba técnica.  
Incluye **base de datos (SQL Server)**, **backend API (.NET 8)** y **frontend (Vue 3 + Pinia)**.

---

##  Estructura del Proyecto
pruebaAsyServys/
├── ControlTemperaturas.API/ # Backend en .NET 8 (API REST)
│ ├── Controllers/ # Controladores
│ ├── Models/ # Entidades y DbContext (EF Core)
│ ├── Program.cs # Configuración principal
│ └── appsettings.json # Configuración (cadena de conexión SQL)
│
├── ControlTemperaturas.Tests/ # Proyecto de pruebas unitarias con xUnit
│ └── ControlEncabezadosTests.cs # Tests básicos para API
│
└── control-temperaturas-frontend/ # Frontend Vue 3 + Pinia
├── src/views/ # Vistas (Formulario.vue, Reporte.vue)
├── src/stores/ # Store Pinia
├── src/services/ # Cliente Axios
└── vite.config.ts # Configuración Vite


##  Endpoints principales
GET /api/ControlEncabezados/Lista
GET /api/ControlEncabezados/Buscar/{id}
POST /api/ControlEncabezados/Guardar
PUT /api/ControlEncabezados/Editar
DELETE /api/ControlEncabezados/Eliminar/{id}
GET /api/ControlEncabezados/Reporte?desde=2025-09-28&hasta=2025-10-01&destino=Planta%20Manta



## Corre el frontend:
cd control-temperaturas-frontend
npm install --force
npm run dev



## Entregables
Base de datos: Cracion tabla, Vista, Sp -- Para la base Datos SQL.sql incluidos.
Backend: .NET 8 con EF Core y Swagger.
Frontend: Vue 3 + Pinia + Axios.
Pruebas: xUnit con dotnet test.
Documentación: este README con pasos de instalación y uso.