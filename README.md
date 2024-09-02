# Documentación sobre el proyecto

En este documento se detallan los features, comentarios y posibles mejoras a futuro sobre el proyecto por pasos.

### EJECUTAR PROYECTO:

El proyecto contiene tres archivos json de configuración, uno general , otro para el modo en desarrollo y otro en producción, el connection string de la base de datos y el secret Key generator para los tokens no deberían exponerse en este archivo, en este caso lo ideal seria usar un proveedor como AZURE KEY VAULT o los secrets key de github mediante flujos de trabajos (workflows in github actions)

Para ejecutar el proyecto deben restaurarse los paquetes con ***dotnet restore***, cambiar el connection string desde la configuración y aplicar **dotnet ef database update** para ejecutar la migración a la base de datos, No se usa scaffolding porque no sigue prácticas a la hora de hacer automatizaciones.

  

### 1. CRUD Operations:

**Productos, Clientes y Ventas: Implementar operaciones CRUD con validación de datos usando FluentValidation.**

La API cuenta con 3 controladores encargados de implementar el CRUD con validaciones, lo ideal sería seguir el Repository Design Pattern para cada Controller. Algunas configuraciones se declaran como métodos de extensión para no alargar el Program.cs y mantener una mejor lógica. La api puede retornar los datos en formato json o en formato xml.

  

### 2. Entity Framework:

**Usar Entity Framework Core para interactuar con la base de datos Oracle Express 21C. - Crear la estructura de las tablas en la base de datos Oracle Express 21 C:**

Para este proyecto, use una base de datos en local, es importante tener en cuenta que hay conversiones de datos que no se aplican correctamente al hacer un migration, como el valor booleano ya que en Oracle solo sería 1 o 0, para ello hice un archivo de configuración por cada tabla especificando reglas y conversiones en el builder del DbContext.

### 3. Validación:

**Implementar validaciones de datos utilizando FluentValidation. Manejar las excepciones a través de middleware global.**

Creé una carpeta para añadir los archivos de validación especificando las reglas con Fluent Validation, Cada Validator se añade por assembly desde el program.cs y posteriormente en el middleware se hace un handle de la excepción si es que ocurre un ValidationException.

### 4. Logging:

**Usar Seq o Serilog para registrar eventos de cada petición a los endpoints.**

El proyecto usa Serilog para los logs, se puede especificar el mínimo nivel de logging que uno necesite y se sobrescribe el default que trae Microsoft, también se genera un archivo log-fecha-dia.txt en una carpeta de logs con un rolling interval por día para tener constancia sobre los logs de nuestro servicio creado. Los errores se manejan mediante el middleware.

### 5. Autenticación y Autorización:

**Implementar autenticación JWT para proteger ciertos endpoints.**

Comenzando por los cors , se habilita a cada controlador el especificado desde el builder. Si el servicio se encuentra en modo desarrollo se habilita para todos los orígenes, métodos y headers, en el caso de que sea producción lo ideal es obtener esos datos desde el archivo de configuración, por ejemplo que solo nuestra aplicación cliente con el forward pueda mandarnos peticiones.

Se añadió **Microsoft.AspNetCore.Identity;** como modelo para la tabla de clientes y JWT como método de seguridad para proteger ciertos endpoint. Se valida issuer , lifetime etc. Como dije anteriormente lo mejor es usar un key vault para generadores y llaves de accesos a servicios.

Por ultimo me gustaría agregar que Añadi un Httpcontext Accesor con un helper en el caso de que se quieran añadir roles al proyecto, lo ideal es hacer endpoints de administrador y endpoints de usuarios y mediante el token obtener datos o Ids importantes para las request como el ROL , EMPRESA , etc.

### 6. Documentación:

**Documentar la API utilizando Swagger/OpenAPI.**

La API esta documentada completamente por tipo de respuesta posible, schemas , versión, etc.

Se especifica en el archivo de extensión la configuración de swagger y activé el generador de xml para generar la documentación mediante comentario xml sobre los endpoints (Manera rápida y conveniente)

Igualmente tiene añadido la definición de seguridad para usar JWT en la UI y poder hacer las pruebas en los endpoints protegidos , entonces no hace falta usar postman por ahora.

### 7. GIT Workflow:

- **Subir el código a GitHub. - Impresiones de pantalla de las pruebas realizadas en Postman o similar.**

- El código se encuentra en este repositorio y las imágenes en la carpeta CAPTURES, estoy disponible ante cualquier consulta o review. ***Muchas gracias!!!***
