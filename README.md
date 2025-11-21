# 🚀 Gestión de Candidatos API

> 📺 **VIDEO DEMO:** Puedes ver la API funcionando y conectada a Base de Datos en este video corto:
> [**Ver Demostración en YouTube**](https://youtu.be/tebnehzpJYc)

API RESTful desarrollada con **.NET 9** para la administración integral de procesos de reclutamiento (Vacantes, Postulantes y Entrevistas).

> **Nota:** Este proyecto fue desarrollado utilizando **ADO.NET puro**, cumpliendo con los requisitos técnicos de alto rendimiento.

---

## 🛠️ Stack Tecnológico

* **Framework:** .NET 9 (ASP.NET Core Web API)
* **Base de Datos:** SQL Server
* **Acceso a Datos:** `Microsoft.Data.SqlClient` (ADO.NET)
* **Documentación:** Swagger UI (OpenAPI)

---

## 🏗️ Arquitectura y Patrones de Diseño

El proyecto sigue una **Arquitectura Limpia** para garantizar la escalabilidad y el mantenimiento:

1. **Patrón Repositorio:** Desacoplamiento total de la lógica de acceso a datos (`Repositorios/` y `Interfaces/`).
2. **Inyección de Dependencias:** Configuración de servicios `Scoped` en el contenedor de .NET.
3. **Programación Asíncrona:** Implementación de `async/await` en todos los controladores y repositorios para evitar bloqueos de hilos (I/O non-blocking).
4. **Manejo de Excepciones:** Control robusto de errores y códigos de estado HTTP (200, 404, 500).

---



## 🗄️ Base de Datos y SQL

El proyecto utiliza **ADO.NET puro** para interactuar con la base de datos y cubrir escenarios reales de manera simple y efectiva:

* ✅ **Consultas Directas (SELECT, INSERT, UPDATE, DELETE):** Implementadas de manera segura con parámetros para evitar SQL Injection.  
* ✅ **Relaciones (Foreign Keys):** Integridad referencial entre `Vacantes` → `Postulantes` → `Entrevistas`.  
* ✅ **Filtros Básicos:** Uso de `WHERE` para filtrar postulantes por vacante y reportes de entrevistas pendientes.

## 🚀 Instrucciones de Instalación

Sigue estos pasos para ejecutar el proyecto localmente:

### 1. Clonar el Repositorio
```bash
git clone https://github.com/OscarAyalaGarcia/GestionCandidatosAPI.git


### 2. Configurar la Base de Datos
1. Abre **SQL Server Management Studio (SSMS)**.
2. Abre el archivo `ScriptBaseDeDatos.sql` que se encuentra en la raíz del proyecto.
3. Ejecuta todo el script para crear la base de datos `GestionCandidatosDB`, las tablas y los procedimientos almacenados.

---

### 3. Configurar la Conexión
Abre el archivo `appsettings.json` y asegúrate de que la cadena de conexión apunte a tu servidor local:

```json
"ConnectionStrings": {
  "CadenaSQL": "Server=.;Database=GestionCandidatosDB;Trusted_Connection=True;TrustServerCertificate=True;"
}

Nota: Si usas SQL Express, cambia Server=. por Server=.\SQLEXPRESS.

## 4. Ejecutar la API

Abre el proyecto en **Visual Studio 2022** (o versión compatible con .NET 8/9 según tu proyecto).

Presiona **F5** o ejecuta el proyecto. La interfaz de **Swagger** se abrirá automáticamente en tu navegador para probar los endpoints.

---

## 📡 Endpoints Principales

| Método | Endpoint | Descripción | SQL Utilizado |
|--------|----------|------------|---------------|
| GET    | /api/Vacantes | Lista todas las vacantes | SELECT * FROM Vacantes |
| GET    | /api/Vacantes/{id} | Obtiene una vacante por Id | SELECT * FROM Vacantes WHERE Id = @id |
| POST   | /api/Vacantes | Crea una vacante | INSERT INTO Vacantes (...) OUTPUT INSERTED.Id |
| PUT    | /api/Vacantes/{id} | Actualiza una vacante | UPDATE Vacantes SET ... WHERE Id = @id |
| DELETE | /api/Vacantes/{id} | Elimina una vacante | DELETE FROM Vacantes WHERE Id = @id |
| GET    | /api/Postulantes | Lista todos los postulantes | SELECT * FROM Postulantes |
| GET    | /api/Postulantes/{id} | Obtiene un postulante por Id | SELECT * FROM Postulantes WHERE Id = @id |
| GET    | /api/Postulantes/PorVacante/{id} | Filtra postulantes por vacante | SELECT * FROM Postulantes WHERE VacanteId = @id |
| POST   | /api/Postulantes | Crea un postulante | INSERT INTO Postulantes (...) OUTPUT INSERTED.Id |
| PUT    | /api/Postulantes/{id} | Actualiza un postulante | UPDATE Postulantes SET ... WHERE Id = @id |
| DELETE | /api/Postulantes/{id} | Elimina un postulante | DELETE FROM Postulantes WHERE Id = @id |
| GET    | /api/Entrevistas | Lista todas las entrevistas pendientes | SELECT con JOIN entre Entrevistas, Postulantes y Vacantes |
| GET    | /api/Entrevistas/{id} | Obtiene entrevista por Id | SELECT * FROM Entrevistas WHERE Id = @id |
| POST   | /api/Entrevistas | Agenda una entrevista | INSERT INTO Entrevistas (...) OUTPUT INSERTED.Id |
| PUT    | /api/Entrevistas/{id} | Actualiza una entrevista | UPDATE Entrevistas SET ... WHERE Id = @id |
| DELETE | /api/Entrevistas/{id} | Elimina una entrevista | DELETE FROM Entrevistas WHERE Id = @id |

---

Desarrollado por **Oscar Alexander Ayala García** como parte del portafolio técnico para desarrollo Backend con .NET y C#.
