using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using MySql.Data.MySqlClient;
using Microsoft.Extensions.Configuration;
using WebApplicationMisCompas.Model;

namespace WebApplicationMisCompas.Pages
{
    public class RegistroModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public RegistroModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [BindProperty]
        public string Name { get; set; }
        [BindProperty]
        public string LastName { get; set; }
        [BindProperty]
        public string Carrera { get; set; }
        [BindProperty]
        public string Email { get; set; }
        [BindProperty]
        public string Password { get; set; }

        int idAlumno;

        public void OnPost()
        {
            string connectionString = _configuration.GetConnectionString("myDb");
            MySqlConnection conexion = new MySqlConnection(connectionString);
            conexion.Open();

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conexion;

            // AlumnoID
            /*
            cmd.CommandText = "SELECT alumnoID FROM alumno ORDER BY alumnoID DESC LIMIT 1";
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    idAlumno = Convert.ToInt32(reader["alumnoID"]) + 1;
                }
                else
                {
                    idAlumno = 1;
                }
            }
            */

            cmd.CommandText = "INSERT INTO alumno (`nombre`, `apellido`, `email`, `password`, `carreraID`) VALUES ('" + Name + "', '" + LastName + "', '" + Email + "', '" + Password + "', '" + Carrera + "');";
            cmd.ExecuteNonQuery();

            cmd.CommandText = "SELECT alumnoID FROM alumno ORDER BY alumnoID DESC LIMIT 1";
            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    idAlumno = Convert.ToInt32(reader["alumnoID"]);
                }
                else
                {
                    idAlumno = 1;
                }
            }

            cmd.CommandText = "INSERT INTO resenia (`calificacion`, `alumnoID`) VALUES ('" + 0 + "', '" + idAlumno + "');";
            cmd.ExecuteNonQuery();

            conexion.Dispose();
            
            Response.Redirect("Menu?");
        }

    }
}
