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
    public class ResenaModel : PageModel
    {
        private readonly IConfiguration _configuration;

        public ResenaModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [BindProperty]
        public int AlumnoID { get; set; }
        [BindProperty]
        public int Calificacion { get; set; }
        [BindProperty]
        public string Descripcion { get; set; }

        public void OnPost()
        {
            string connectionString = _configuration.GetConnectionString("myDb");
            MySqlConnection conexion = new MySqlConnection(connectionString);
            conexion.Open();

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conexion;

            cmd.CommandText = "INSERT INTO resenia (`calificacion`, `descripcion`, `alumnoID`) VALUES ('" + Calificacion + "', '" + Descripcion + "', '" + AlumnoID + "');";
            cmd.ExecuteNonQuery();

            conexion.Dispose();
            Response.Redirect("Menu?");
        }
    }
}
