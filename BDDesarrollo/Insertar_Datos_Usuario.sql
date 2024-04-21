USE [sistemaAsesorias]
GO

/*
Registrarlos roles Administrador y Asesor a la tabla rol
*/
INSERT INTO rol VALUES (1,'Administrador')
GO
INSERT INTO rol VALUES (2,'Asesor')
GO

/*
Registrar Administradores en la tabla administrador
*/

INSERT INTO administrador VALUES (100,'','','','','contrasena')
GO

INSERT INTO administrador VALUES (200,'Olmos','Rodriguez','Kevin David','kevin.olmos@uabc.edu.mx','contrasena')
GO

INSERT INTO administrador VALUES (300,'Lara','Gutierrez','Carlos Daniel','carlos.daniel.lara.gutierrez@uabc.edu.mx','contrasena')
GO

INSERT INTO administrador VALUES (400,'Avendaño','Sánchez','Acis','acis.avendano@uabc.edu.mx','contrasena')
GO


/*
Registrar los 10 primeros alumnos de la tabla alumnos a la tabla asesor
*/

INSERT INTO asesor (matricula, contraseña)
SELECT TOP 10 matricula, CONCAT('AL',CAST (matricula AS VARCHAR(50))) as contraseña 
FROM alumno


/*
Registro de los usuarios en la tabla Usuarios
*/
INSERT INTO usuario (matricula,idRol) 
SELECT CAST (matricula AS VARCHAR(50)), idRol 
FROM administrador, rol  
WHERE idRol=1

/*
Registro de los asesores en la tabla Usuarios
*/

INSERT INTO usuario (matricula,idRol) 
SELECT CAST (matricula AS VARCHAR(50)), idRol 
FROM asesor, rol  
WHERE idRol=2

