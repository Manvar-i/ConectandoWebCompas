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
    public class BorraModel : PageModel
    {
        private readonly IConfiguration _configuration;


        public BorraModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public void getPerfilMatar(int aidi)
        {
            string connectionString = _configuration.GetConnectionString("myDb");

            MySqlConnection conexion = new MySqlConnection(connectionString);
            conexion.Open();

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conexion;

            // Datos del alumno - CARRERA
            cmd.CommandText = "DELETE FROM resenia WHERE alumnoID = '" + aidi + "';" +
                              "DELETE FROM alumno WHERE alumnoID = '" + aidi + "';";
            cmd.ExecuteNonQuery();
            conexion.Dispose();

            Response.Redirect("Menu?");
        }


        public void OnPost(string aidi)
        {
            getPerfilMatar(Convert.ToInt32(aidi));
        }
    }
}
