using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using WebApplicationMisCompas.Model;

namespace WebApplicationMisCompas.Pages
{
    public class MenuModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public IList<Alumno> ListaAlumnos { get; set; }

        public MenuModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private void cargarDatos()
        {
            string connectionString = _configuration.GetConnectionString("myDb");

            MySqlConnection conexion = new MySqlConnection(connectionString);
            conexion.Open();

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conexion;

            Alumno alumno1 = new Alumno();
            ListaAlumnos = new List<Alumno>();

            cmd.CommandText = "SELECT alumno.alumnoID, alumno.nombre, alumno.apellido, alumno.email, carrera.nombreSiglas, (SUM(resenia.calificacion)/(COUNT(resenia.calificacion)-1)) AS promedio FROM alumno INNER JOIN carrera ON alumno.carreraID = carrera.carreraID INNER JOIN resenia ON resenia.alumnoID = alumno.alumnoID GROUP BY alumnoID;";

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    alumno1 = new Alumno();
                    alumno1.ApplicantID = Convert.ToInt32(reader["alumnoID"]);
                    alumno1.Name = reader["nombre"].ToString();
                    alumno1.LastName = reader["apellido"].ToString();
                    alumno1.Email = reader["email"].ToString();
                    alumno1.NombreCarrera = reader["nombreSiglas"].ToString();
                    alumno1.Promedio = reader["promedio"].ToString();
                    ListaAlumnos.Add(alumno1);
                }
            }
            cmd.ExecuteNonQuery();
        }

        public void OnPost()
        {
            cargarDatos();
        }

        public void OnGet()
        {
            cargarDatos();
        }
    }
}
