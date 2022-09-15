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
    public class EditarModel : PageModel
    {
        private readonly IConfiguration _configuration;

        [BindProperty]
        public int Carrera { get; set; }


        public EditarModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public void getPerfilCarrera(int aidi)
        {
           
            string connectionString = _configuration.GetConnectionString("myDb");

            MySqlConnection conexion = new MySqlConnection(connectionString);
            conexion.Open();

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conexion;

            // Datos del alumno - CARRERA
            cmd.CommandText = "UPDATE `alumno` SET `carreraID` = '" + Carrera + "' WHERE alumnoID = '" + aidi + "';";
            cmd.ExecuteNonQuery();
            conexion.Dispose();

            Response.Redirect("Menu?");
        }

        public void OnPost(string aidi)
        {

            getPerfilCarrera(Convert.ToInt32(aidi));
        }

    }
}
