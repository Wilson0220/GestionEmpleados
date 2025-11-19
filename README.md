Gestión de Empleados
Sistema para la gestión de empleados, departamentos y estados laborales dentro de una empresa.
Incluye una API RESTful en .NET, base de datos en SQL Server y un frontend en Vue 3, organizados en una estructura modular y mantenible.

Tecnologías utilizadas
  Backend (API .NET)
    .NET 8
    C#
    Web API
    Entity Framework Core
    SQL Server
    Swagger (Documentación API)
  
  Frontend
    Vue 3
    Vite
    TypeScript
    Axios
  
  Base de Datos
    SQL Server
    Tablas:
      Empleados
      Departamentos
      EstadosEmpleado

Instalación del Backend (.NET API)
  1. Entrar a la carpeta backend
     cd backend
  2. Restaurar paquetes
     dotnet restore
  3. Configurar cadena de conexión en appsettings.json
     "ConnectionStrings": {"DefaultConnection": "Server=localhost;Database=GestionEmpleadosDB;Trusted_Connection=True;"}
  4.Ejecutar la API
    dotnet run  
  5.La API se ejecutará típicamente en:
    https://localhost:5001

Instalación del Frontend (Vue + Vite)
  1. Entrar a la carpeta frontend
     cd frontend
  2. Instalar dependencias
     npm install
  3. Ejecutar el proyecto
     npm run dev
  4.El frontend normalmente corre en:
    http://localhost:5173

Conexión Frontend → Backend
  const api = axios.create({
    baseURL: "https://localhost:5001/api",
  });

Cómo correr el sistema completo?
  Iniciar el Backend
    cd backend
    dotnet run  
  Iniciar el Frontend
    cd frontend
    npm run dev  
  Abrir en el navegador
    http://localhost:5173

Mejoras Futuras
  Autenticación y autorización (JWT + Roles)
  Implementación de CI/CD
  Integración con Docker

Autor
  Wilson Féliz
  Proyecto desarrollado como práctica profesional para gestión moderna de empleados utilizando .NET, SQL Server y Vue.js.
