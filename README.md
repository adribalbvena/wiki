# wiki
## Objetivos 馃搵
Desarrollar un sistema, que permita la administraci贸n general de una Wiki (de cara a los administradores): Usuarios Autores, Articulos con Encabezados, cuerpo, Entradas, Referencias, Mensajes, etc., como as铆 tambi茅n, permitir a los usuarios Lectores, navegar la Wiki y enviar mensajes.
Utilizar Visual Studio 2019 preferentemente y crear una aplicaci贸n utilizando ASP.NET MVC Core 3.1

<hr />

## Enunciado 馃摙
La idea principal de este trabajo pr谩ctico, es que Uds. se comporten como un equipo de desarrollo.
Este documento, les acerca, un equivalente al resultado de una primera entrevista entre el cliente y alguien del equipo, el cual relev贸 e identific贸 la informaci贸n aqu铆 contenida. 
A partir de este momento, deber谩n comprender lo que se est谩 requiriendo y construir dicha aplicaci贸n, 

Deben recopilar todas las dudas que tengan y evacuarlas con su nexo (el docente) de cara al cliente. De esta manera, 茅l nos ayudar谩 a conseguir la informaci贸n ya un poco m谩s procesada. 
Es importante destacar, que este proceso, no debe esperar a ser en clase; es importante, que junten algunas consultas, sea de 铆ndole funcional o t茅cnicas, en lugar de cada consulta enviarla de forma independiente.

Las consultas que sean realizadas por correo deben seguir el siguiente formato:

Subject: [NT1-<CURSO LETRA>-GRP-<GRUPO NUMERO>] <Proyecto XXX> | Informativo o Consulta

Body: 

1.<xxxxxxxx>

2.< xxxxxxxx>


# Ejemplo
**Subject:** [NT1-A-GRP-5] Agenda de Turnos | Consulta

**Body:**

1.La relaci贸n del paciente con Turno es 1:1 o 1:N?

2.Est谩 bien que encaremos la validaci贸n del turno activo, con una propiedad booleana en el Turno?

<hr />

### Proceso de ejecuci贸n en alto nivel 鈽戯笍
 - Crear un nuevo proyecto en [visual studio](https://visualstudio.microsoft.com/en/vs/).
 - Adicionar todos los modelos dentro de la carpeta Models cada uno en un archivo separado.
 - Especificar todas las restricciones y validaciones solicitadas a cada una de las entidades. [DataAnnotations](https://docs.microsoft.com/en-us/dotnet/api/system.componentmodel.dataannotations?view=netcore-3.1).
 - Crear las relaciones entre las entidades
 - Crear una carpeta Data que dentro tendr谩 al menos la clase que representar谩 el contexto de la base de datos DbContext. 
 - Crear el DbContext utilizando base de datos en memoria (con fines de testing inicial). [DbContext](https://docs.microsoft.com/en-us/dotnet/api/microsoft.entityframeworkcore.dbcontext?view=efcore-3.1), [Database In-Memory](https://docs.microsoft.com/en-us/ef/core/providers/in-memory/?tabs=vs).
 - Agregar los DbSet para cada una de las entidades en el DbContext.
 - Crear el Scaffolding para permitir los CRUD de las entidades al menos solicitadas en el enunciado.
 - Aplicar las adecuaciones y validaciones necesarias en los controladores.  
 - Realizar un sistema de login con al menos los roles equivalentes a <Usuario Cliente> y <Usuario Administrador> (o con permisos elevados).
 - Si el proyecto lo requiere, generar el proceso de registraci贸n. 
 - Un administrador podr谩 realizar todas tareas que impliquen interacci贸n del lado del negocio (ABM "Alta-Baja-Modificaci贸n" de las entidades del sistema y configuraciones en caso de ser necesarias).
 - El <Usuario Cliente> s贸lo podr谩 tomar acci贸n en el sistema, en base al rol que tiene.
 - Realizar todos los ajustes necesarios en los modelos y/o funcionalidades.
 - Realizar los ajustes requeridos del lado de los permisos.
 - Todo lo referido a la presentaci贸n de la aplicai贸n (cuestiones visuales).
 
<hr />

## Entidades 馃搫

- Autor
- Articulo
- Encabezado
- Cuerpo
- Entrada
- Referencia
- Mensaje

`Importante: Todas las entidades deben tener su identificador unico. Id o <ClassNameId>`

`
Las propiedades descriptas a continuaci贸n, son las minimas que deben tener las entidades. Uds. pueden agregar las que consideren necesarias.
De la misma manera Uds. deben definir los tipos de datos asociados a cada una de ellas, como as铆 tambi茅n las restricciones.
`

**Usuario**
```
- Nombre
- Apellido
- Email
- Telefono
- FechaAlta
- Password
- Rol
```
>:question:
>驴Existe la clase usuario?

>:white_check_mark:
>Se defini贸 a la clase Usuario como base de Autor, no existe la clase Administrador.

>:white_check_mark:
>Los usuarios poseen un Rol, el cual inicia como Autor en caso de ser una autoregistraci贸n

**Autor** ()
```
- Nombre
- Apellido
- Email
- Telefono
- FechaAlta
- Password
- Articulos
```
>:question:
>驴El Autor es un Usuario? 驴O posee una referencia a dicho Usuario?

>:white_check_mark:
>Se defini贸 a la clase Usuario como base de Autor.

**Articulo**
```
- Fecha
- Activo
- CategoriaPrincipal
- CategoriasSecundarias
- Autor
- Encabezado
- Cuerpo
- Referencias
- Mensajes
- PalabrasClave
```
>:question:
>驴Puede un art铆culo no tener cartegor铆as secundarias, referencias y/o palabras clave?

>:white_check_mark:
>Las categor铆as secundarias y las referencias no son obligatorias. Sin embargo, todos los art铆culos deben tener, c贸mo m铆nimo, 3 palabras clave.

>:white_check_mark:
>La categoria principal es obligatoria.

**Categoria**
```
- Nombre
- Activa
- Descripcion
- Articulos
```

**Encabezado**
```
- Titulo
- Descripcion
- Articulo
```

**Cuerpo**
```
- Entradas
- Articulo
```

**Entrada**
```
- Orden
- Titulo
- Subtitulo
- Texto
- Cuerpo
```
>:question:
>驴Qu茅 representa el art铆culo en la clase Entrada? 驴Es el art铆culo al cual pertenece? 驴No deber铆a ser una referencia al cuerpo de ese articulo, que es en definitiva el que contiene las entradas?

>:white_check_mark:
>Se modific贸 la clase Entrada para hacer referencia al cuerpo y no al art铆culo.


**Referencia**
```
- ArticuloPrincipal
- ArticuloReferencial
```
>:question:
>De la clase Referencia, 驴el art铆culo principal es el que contiene la referencia y el art铆culo referencial es al que apunta dicha referencia, o viceversa?

>:white_check_mark:
>ArticuloPrincipal es aquel que contiene una o muchas referencias a otro/s, representados por ArticuloReferencial. En otras palabras, Referencia, es una entidad intermedia para hacer un Muchos a Muchos para la entidad Articulo, con si misma.

**Mensaje**
```
- FechaYHora
- Articulo
- Usuario
- Titulo
- Texto
```

**PalabraClave**
```
- Palabra
- Articulos
```

**NOTA:** aqu铆 un link para refrescar el uso de los [Data annotations](https://www.c-sharpcorner.com/UploadFile/af66b7/data-annotations-for-mvc/).

<hr />

## Caracteristicas y Funcionalidades 鈱笍
`Todas las entidades, deben tener implementado su correspondiente ABM, a menos que sea implicito el no tener que soportar alguna de estas acciones.`

**Usuario**
- Los Usuarios pueden auto registrarse.
- La autoregistraci贸n desde el sitio, es exclusiva para los usuarios externos. Por lo cual, se le asignar谩 dicho rol. (hay que diferenciarlo? o solo con ser Usuario alcanza?)
- Los administradores de la Wiki, deben ser agregados por otro Administrador.
	- Al momento, del alta del Administrador, se le definir谩 un username y password.
    - Tambi茅n se le asignar谩 a estas cuentas el rol de Administrador.
- Los usuarios pueden navegar por la wiki y postear mensajes en los articulos.
- Pueden crear Articulos. (si los usuarios pueden crear articulos, de qu茅 sirve el Autor?)
- Pueden solicitar la creaci贸n de un Categoria.

**Autor**
- Un Autor puede crear un articulo (la creaci贸n de un articulo como usuario, no es con wizard?)
    - El proceso ser谩 en modo Wizard.
        - Selecciona la categoria existente.
        -- Si no existe la categoria que quiere, puede solicitar la creaci贸n de una, mediante la carga una que no estar谩 activa.
        --- El proceso ser谩 igual que la creaci贸n de una nueva categoria, con la diferencia de: 1. Estar谩 desactivada, hasta que un administrador la active. 2. Quedar谩 un registro del usuario solicitante, para notificarlo con el resultado de su solicitud. 
        - Puede seleccionar m谩s categorias como secundarias.
        - Crea un encabezado con su contenido
        - Hasta aqu铆 es lo minimo requerido para crear un articulo.
        - Luego se le consulta, si quiere crear una entrada en el Articulo.
        -- Al finalizar, le vuelve a consultar si quiere agregar m谩s entradas y repetir el proceso.
        -- Cada entrada agregada, quedar谩 debajo de la anterior al visualizarse en el articulo.
        - Puede agregar referencias a otros articulos de la wiki. Para ello, se le ofrecer谩 agregar referencias y mediante un buscador, podr谩 seleccionar articulos para generar una referencia.
        - Tambi茅n, podr谩 agregar palabras clave, que sirvan para que encuentren su articulo. Al agregar una palabra clave, buscar谩 una similitud en la base de palabras y propondr谩 hasta 10, El autor puede seleccionar una o crear una.
        - El articulo estar谩 por defecto Inactivo.
- El Autor, puede activar y desactivar sus articulos cuando lo desee.
- Puede actualizar datos de contacto etc.

**Administrador**
- Un Administrador, puede crear Categorias.
- Un Administrador, puede desactivar articulos de los usuarios.
- Puede activar categorias propuestas por los usuarios.
- Puede crear palabras clave

>:question:
>驴Pueden los administradores crear art铆culos?
>:white_check_mark:
>Los administradores no pueden crear art铆culos.

**Articulo**
- El articulo tendr谩 una categoria principal, y puede tener categorias secundarias.
-- Un articulo, no puede tener dos veces una categoria como principal y como secundaria, o dos veces como secundaria.
- El articulo cargar谩 automaticamente la fecha de creaci贸n.
- Por defecto, los articulos estar谩n Inactivos, y por consiguiente, no estar谩n disponibles para ser visualizados y o encontrados por el buscador.
- Solo puede tener un Autor
- Solo puede tener un Encabezado
- Solo puede tener un Cuerpo, el cual podr谩 tener muchas entradas. Ordenadas, en el orden que se fueron agregando.
- Las Referencias, ser谩n visualizadas como links, a otros articulos.
- Los Mensajes ser谩n visualizados, en orden creciente por fecha de carga. 
- Las PalabrasClave, serviran para encontrar el articulo, y ser谩n visualizadas tambi茅n como links, a un listado de articulos relacionados.

**Aplicaci贸n General**
- La wiki, mostrar谩 los encabezados en la home de los ultimos 4 articulos creados. Para que los usuarios puedan acceder a verlos.
- Se debe ofrecer tambi茅n, navegaci贸n por categorias. 
- Se debe ofrecer un buscador, por palabras clave, que ofrecer谩 un listado sabana, de articulos relacionados. Hasta 10.
- Se debe listar el top 3 de autores con m谩s articulos.
- Los usuarios no pueden eliminar los articulos, solo pueden deshabilitarse.
- Los administradores, si pueden eliminar los articulos.
- Los accesos a las funcionalidades y/o capacidades, debe estar basada en los roles que tenga cada individuo.
