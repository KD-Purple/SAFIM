$sqlcmdpath = "/opt/mssql-tools/bin/sqlcmd";
$scriptpath = "/var/opt/mssql/scripts";
$ruta = ${PWD} -replace "\\","/";

if (Test-Path -Path .\BDDesarrollo\Respaldos) {
    Write-Output "Ya existe la carpeta de Respaldos";
}
else {
    mkdir .\BDDesarrollo\Respaldos
}

docker volume create SQLS2019;

(Get-Content ${PWD}\docker-compose.yml).replace("C:/<ruta donde clono el proyecto>",$ruta) | Set-Content ${PWD}\docker-compose.yml

docker-compose up -d;

Write-Output "Esperando que se termine de inicializar la base de datos, 30 segundos de espera";

Start-Sleep 30;

docker exec MSSQLS_2019 ${sqlcmdpath} -U sa -P EntornoSis1 -d master -i ${scriptpath}/Crear_BD.sql;
docker exec MSSQLS_2019 ${sqlcmdpath} -U sa -P EntornoSis1 -d master -i ${scriptpath}/Insertar_Datos_Materia.sql;
docker exec MSSQLS_2019 ${sqlcmdpath} -U sa -P EntornoSis1 -d master -i ${scriptpath}/Insertar_Datos_Alumno.sql;
docker exec MSSQLS_2019 ${sqlcmdpath} -U sa -P EntornoSis1 -d master -i ${scriptpath}/Insertar_Datos_Usuario.sql;
docker exec MSSQLS_2019 ${sqlcmdpath} -U sa -P EntornoSis1 -d master -i ${scriptpath}/Insertar_Datos_Asesoria.sql;