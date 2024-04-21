FROM mcr.microsoft.com/mssql/server:2019-latest as bd

ENV ACCEPT_EULA=Y 
ENV MSSQL_PID=Express
ENV MSSQL_SA_PASSWORD=EntornoSis1
ENV MSSQL_LCID=3082
ENV MSSQL_COLLATION=Modern_Spanish_CI_AS 
ENV MSSQL_DATA_DIR=/var/opt/mssql/data
ENV MSSQL_LOG_DIR=/var/opt/mssql/log
ENV MSSQL_BACKUP_DIR=/var/opt/mssql/backup

RUN mkdir /var/opt/mssql/backup
RUN mkdir /var/opt/mssql/scripts

COPY . /var/opt/mssql/scripts