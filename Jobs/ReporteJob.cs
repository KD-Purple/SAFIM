using Azure.Core.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using SAFIM.Models;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;

namespace SAFIM.Jobs
{
    public class ReporteJob : BackgroundService
    {

        private readonly SistemaAsesoriasContext _contexto;
        private Timer? _timer;

        public ReporteJob(IServiceScopeFactory contexto)
        {
            _contexto = contexto.CreateScope().ServiceProvider.GetRequiredService<SistemaAsesoriasContext>();
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //Temporizador en Minutos
            _timer = new Timer(Dowork, null, TimeSpan.Zero, TimeSpan.FromMinutes(30));
        }

        private void Dowork(object? state)
        {
            //Obtener el nombre del día actual desde el sistema 
            String dia = System.DateTime.Now.ToString("dddd", new CultureInfo("es-MX"));
            //Variable hora para saber la hora actual, esta hora es obtenida por el reloj de la computadora
            TimeSpan hora = TimeSpan.ParseExact(DateTime.Now.ToString("HH:mm"), "g", CultureInfo.InvariantCulture);
            //Variable consulta la cual tendra almacenada la consulta asesoria para realizar consultas en el switch
            var consulta = _contexto.Asesoria.AsQueryable();
            //Variable rep abreviatura de reporte, la cual almacenara las consultas generadas
            //por el switch correspondiente al día actual
            List<Reporte>? rep = null;

            //Switch por días de la semana para guardar en una lista basada en el modelo reporte los datos principales del reporte
            //tales como el IdAsesoria, MatriculaAsesor, ClaveMateria y Horario
            switch (dia)
            {
                case "lunes":
                    rep = consulta
                        .Where(a => a.HorarioLunes != null)
                        .Select(a => new Reporte()
                        {
                            IdAsesoria = a.IdAsesoria,
                            MatriculaAsesor = a.MatriculaAsesor,
                            ClaveMateria = a.ClaveMateria,
                            Horario = a.HorarioLunes
                        }).ToList();
                    break;
                case "martes":
                    rep = consulta
                        .Where(a => a.HorarioMartes != null)
                        .Select(a => new Reporte()
                        {
                            IdAsesoria = a.IdAsesoria,
                            MatriculaAsesor = a.MatriculaAsesor,
                            ClaveMateria = a.ClaveMateria,
                            Horario = a.HorarioMartes
                        }).ToList();
                    break;
                case "miércoles":
                    rep = consulta
                        .Where(a => a.HorarioMiercoles != null)
                        .Select(a => new Reporte()
                        {
                            IdAsesoria = a.IdAsesoria,
                            MatriculaAsesor = a.MatriculaAsesor,
                            ClaveMateria = a.ClaveMateria,
                            Horario = a.HorarioMiercoles
                        }).ToList();
                    break;
                case "jueves":
                    rep = consulta
                        .Where(a => a.HorarioJueves != null)
                        .Select(a => new Reporte()
                        {
                            IdAsesoria = a.IdAsesoria,
                            MatriculaAsesor = a.MatriculaAsesor,
                            ClaveMateria = a.ClaveMateria,
                            Horario = a.HorarioJueves
                        }).ToList();
                    break;
                case "viernes":
                    rep = consulta
                        .Where(a => a.HorarioViernes != null)
                        .Select(a => new Reporte()
                        {
                            IdAsesoria = a.IdAsesoria,
                            MatriculaAsesor = a.MatriculaAsesor,
                            ClaveMateria = a.ClaveMateria,
                            Horario = a.HorarioViernes
                        }).ToList();
                    break;
            }

            if (rep != null)
                foreach (var a in rep)
                {
                    TimeSpan horario = TimeSpan.ParseExact(a.Horario.Split(" a ")[1], "g", CultureInfo.InvariantCulture);
                    if (TimeSpan.Compare(hora, horario) >= 0)
                    {

                        DateTime fecha = System.DateTime.Now.Date;
                        var validar = !_contexto.Reporte.Where(r => r.IdAsesoria == a.IdAsesoria)
                        .Where(r => r.Fecha.CompareTo(fecha) == 0).IsNullOrEmpty();

                        if (validar)
                        {
                            _contexto.Add(new Reporte
                            {
                                IdAsesoria = a.IdAsesoria,
                                MatriculaAsesor = a.MatriculaAsesor,
                                ClaveMateria = a.ClaveMateria,
                                Horario = a.Horario.Split(" a ")[1],
                                Fecha = fecha

                            });
                            _contexto.SaveChanges();
                        }
                    }

                }
        }

        public override async Task StopAsync(CancellationToken stoppingToken)
        {
        }
    }




}
