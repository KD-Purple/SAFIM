/* Creacion de la base de datos sistemaAsesorias*/
USE [master]
DROP DATABASE [sistemaAsesorias]

CREATE DATABASE sistemaAsesorias ON
(
    NAME = sistemaAsesorias,
    FILENAME = '/var/opt/mssql/data/sistemaAsesorias.mdf',
    SIZE = 1024KB,
    MAXSIZE = 4096MB,
    FILEGROWTH = 30%
)
LOG ON
(
    NAME = sistemaAsesorias_log,
    FILENAME = '/var/opt/mssql/log/sistemaAsesorias.ldf',
    SIZE = 10MB,
    MAXSIZE = 2048MB,
    FILEGROWTH = 10%
);
GO
/* Fin de la creacion de la base de datos sistemaAsesorias*/

/*--***************************************************************************--*/

/* Inicio de la configuracion de la base de datos sistemaAsesorias*/

ALTER DATABASE [sistemaAsesorias] SET RECOVERY FULL
GO

ALTER DATABASE [sistemaAsesorias] SET  READ_WRITE 
GO

/* Fin de la configuracion de la base de datos*/

/*--***************************************************************************--*/

/* Crear tablas dentro de la base de datos de sistemaAsesorias */
USE [sistemaAsesorias]
GO

/* 
Tablas que se deben crear primero
	- administrador
	- alumno
	- rol
	- materia
Tablas que se deben crear despues
	-usuarios
	-asesor
	-asesoria
	-reportes
*/

/*--
Esta tabla almacenara todos los datos esenciales del adminsitrador,
matricula, apellido paterno, apellido materno, nombre,
correo y la contraseña para acceder al sistema
--*/

-- Esta tabla no usa llaves foraneas

CREATE TABLE [dbo].[administrador](
	[matricula] [int] NOT NULL,
	[apellidoPaterno] [varchar](50) NOT NULL,
	[apellidoMaterno] [varchar](50) NOT NULL,
	[nombreAdmin] [varchar](50) NOT NULL,
	[correo] [varchar](50) NOT NULL,
	[contraseña] [varchar](50) NOT NULL,
    CONSTRAINT PK_administrador_matricula 
	PRIMARY KEY CLUSTERED (matricula)
)
GO

-- Crear Tabla de alumno

/*--
Esta tabla almacenara todos los datos esenciales del alumno,
matricula, apellido paterno, apellido materno, nombre,
correo, carrera y el semestre en el que se encuentra actualmente
--*/

-- Esta tabla no usa llaves foraneas

CREATE TABLE [dbo].[alumno](
	[matricula] [int] NOT NULL,
	[apellidoP] [varchar](50) NOT NULL,
	[apellidoM] [varchar](50) NOT NULL,
	[nombreAlumno] [varchar](50) NOT NULL,
	[correo] [varchar](50) NOT NULL,
	[carrera] [varchar](max) NOT NULL,
	[semestre] [int] NOT NULL,
    CONSTRAINT PK_alumno_matricula 
	PRIMARY KEY CLUSTERED (matricula)
)
GO

-- Crear tabla rol

CREATE TABLE [dbo].[rol](
	[idRol] [int] NOT NULL,
	[descripcion] [varchar](50) NULL,
    CONSTRAINT PK_rol_idRol 
	PRIMARY KEY CLUSTERED (idRol)
)
GO

-- Crear tabla materia

-- Esta tabla no usa llaves foraneas

CREATE TABLE [dbo].[materia](
	[claveMateria] [int] NOT NULL,
	[nombreMateria] [varchar](max) NOT NULL,
    CONSTRAINT PK_materia_claveMateria 
	PRIMARY KEY CLUSTERED (claveMateria)
)


-- Crear tabla asesor

/*--
Esta tabla almacenara el id del asesor, matricula y la contraseña con
la cual accedera al sistema, la matricula esta asociada a la matricula
de la tabla alumno
--*/

/* Llaves Foraneas 
    |- alumno.matricula
*/

CREATE TABLE [dbo].[asesor](
	[matricula] [int] NOT NULL ,
	[contraseña] [varchar](50) NOT NULL,
    CONSTRAINT PK_asesor_matricula 
	PRIMARY KEY CLUSTERED (matricula),
	CONSTRAINT FK_asesor_matricula 
	FOREIGN KEY (matricula) REFERENCES alumno(matricula)
)
GO

-- Crear tabla usuarios

/* Llaves Foraneas 
    |- rol.idRol
*/

CREATE TABLE [dbo].[usuario](
	[matricula] [varchar](50) NOT NULL,
	[idRol] [int] NULL,
    CONSTRAINT PK_usuarios_matricula 
	PRIMARY KEY CLUSTERED (matricula),
	CONSTRAINT FK_usuario_idRol
	FOREIGN KEY (idRol) REFERENCES rol(idRol)
)
GO

-- Crear tabla asesoria

/* Llaves Foraneas 
    |- asesor.matricula
    |- materia.claveMateria
*/

CREATE TABLE [dbo].[asesoria](
    [idAsesoria] [int] IDENTITY(1,1) NOT NULL,
	[matriculaAsesor] [int] NOT NULL ,
	[claveMateria] [int] NOT NULL,
	[horarioLunes] [varchar](max),
	[horarioMartes] [varchar](max),
	[horarioMiercoles] [varchar](max),
	[horarioJueves] [varchar](max),
	[horarioViernes] [varchar](max),
    CONSTRAINT PK_asesoria_idAsesoria 
	PRIMARY KEY CLUSTERED (idAsesoria),
	CONSTRAINT FK_asesoria_matriculaAsesor
	FOREIGN KEY (matriculaAsesor) REFERENCES asesor(matricula),
	CONSTRAINT FK_asesoria_claveMateria
	FOREIGN KEY (claveMateria) REFERENCES materia(claveMateria),
)
GO

-- Crear tabla reporte


/* Llaves Foraneas 
    |- alumno.matricula
    |- materia.claveMateria
*/

CREATE TABLE [dbo].[reporte](
	[idReporte] [int] IDENTITY(1,1) NOT NULL,
	[idAsesoria] [int] NOT NULL,
	[matriculaAsesor] [int] NOT NULL,
	[claveMateria] [int] NOT NULL,
	[horario][varchar](30),
	[fecha] [date] NOT NULL,
    CONSTRAINT PK_reporte_idReporte 
	PRIMARY KEY CLUSTERED (idReporte),
	CONSTRAINT FK_reporte_matriculaAsesor 
	FOREIGN KEY (matriculaAsesor) REFERENCES alumno(matricula),
	CONSTRAINT FK_reporte_matricula 
	FOREIGN KEY (claveMateria) REFERENCES materia(claveMateria),
)
GO


CREATE TABLE [dbo].[detallereporte](
	[idDetalle] [int] IDENTITY(1,1) NOT NULL,
	[idReporte] [int] NOT NULL,
	[matriculaAlumno] [int],
	[grupo][int],
	[programaEducativo][varchar](50),
	[temas] [varchar](max),
	[tiempo] [varchar](30) ,
	[comentarios] [varchar](max),
    CONSTRAINT PK_reporte_idDetalle 
	PRIMARY KEY CLUSTERED (idDetalle),
	CONSTRAINT FK_reporte_idreporte 
	FOREIGN KEY (idReporte) REFERENCES reporte(idReporte) ON DELETE CASCADE,
	CONSTRAINT FK_reporte_matriculaAlumno 
	FOREIGN KEY (matriculaAlumno) REFERENCES alumno(matricula),
)
GO
/* Fin de la creacion de tablas*/

/*--***************************************************************************--*/
