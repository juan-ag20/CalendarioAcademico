# AulaAgenda — Calendario académico
Proyecto ASP.NET Core MVC para Visual Studio, dirigido a .NET 8.

## Abrir y ejecutar
1. Instala Visual Studio con la carga de trabajo **ASP.NET y desarrollo web** y el SDK de .NET 8. Si tu Visual Studio no reconoce net8.0, actualízalo o instala el SDK correspondiente.
2. Extrae todo el ZIP a una carpeta local.
3. Abre `CalendarioAcademico.sln`.
4. Selecciona CalendarioAcademico como proyecto de inicio y pulsa F5 o Ctrl+F5.
5. Si Visual Studio pregunta por el certificado HTTPS de desarrollo, acepta confiar en él.
6. Crea tu cuenta. Registra una materia y luego una tarea, examen o apunte.

También puedes ejecutar `dotnet run --project CalendarioAcademico.csproj` desde la carpeta del proyecto.

## Funciones de esta versión
- Registro de nombre y apellido en un único campo, correo único y contraseña de mínimo 8 caracteres.
- Inicio y cierre de sesión con cookies. Contraseñas con PasswordHasher de ASP.NET Core.
- Calendario mensual, navegación entre meses, botón Hoy y acceso al detalle editable de cada evento.
- Registro, consulta, edición y eliminación de materias (profesor, horario, salón y descripción).
- Espacio de materia a través de su listado filtrado de actividades.
- Tareas con prioridad, descripción, fecha y estado; exámenes con fecha, hora y lugar; apuntes con título y contenido.
- Recordatorios visuales de hoy y los próximos siete días y aviso de actividades vencidas. Se actualizan al recargar o guardar.
- Información separada por cuenta. Validaciones en servidor y protección antifalsificación en formularios POST.
- Interfaz adaptable a pantallas pequeñas, en español y sin dependencias de CDN.

## Persistencia
Se crea automáticamente `App_Data/calendar.json`. Los datos sobreviven al reiniciar. No edites ni elimines el archivo mientras la aplicación esté en uso. Guarda una copia de esa carpeta como respaldo; contiene datos personales y hashes de contraseña. Las fechas son horas locales ingresadas por el estudiante, sin conversión de zona horaria.

## Alcance y límites
Esta es una primera versión académica para ejecución local en una sola instancia. No requiere SQL Server ni paquetes NuGet adicionales. La recuperación de contraseña por correo, un perfil editable, avisos por correo/push y servicios programados no están implementados. Los recordatorios solo se ven al abrir el sitio. Antes de un despliegue público, sustituye JSON por una base de datos, implementa recuperación y verificación de correo, límites de intentos de acceso y configuración de producción.

## Organización
- Models: entidades y validaciones.
- Controllers: autenticación y operaciones del usuario.
- Services/Store.cs: lectura y escritura de JSON con exclusión mutua y reemplazo de archivo.
- Views: vistas Razor.
- wwwroot/css/site.css: diseño visual.

## Verificación de entrega
Se revisaron referencias de vistas, rutas y estructura del ZIP. Este entorno no dispone del SDK de .NET; no se pudo compilar ni ejecutar el proyecto aquí. La compilación y prueba de extremo a extremo deben realizarse en Visual Studio.
