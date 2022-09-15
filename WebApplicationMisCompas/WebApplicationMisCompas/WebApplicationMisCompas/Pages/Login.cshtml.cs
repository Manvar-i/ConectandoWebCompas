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
    public class LoginModel : PageModel
    {
        private readonly IConfiguration _configuration;
        [BindProperty]
        public string Email { get; set; }
        [BindProperty]
        public string Password { get; set; }

        public LoginModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void OnPost()
        {
            string connectionString = _configuration.GetConnectionString("myDb");
            MySqlConnection conexion = new MySqlConnection(connectionString);
            conexion.Open();

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conexion;
            cmd.CommandText = "SELECT * FROM alumno WHERE email = '" + Email + "' AND password = '" + Password + "'";

            using (var reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    Response.Redirect("PerfilEditable?Email=" + Email);
                }
                else
                {
                    ViewData["Mensaje"] = "Email o password incorrectos";
                }
            }
            conexion.Dispose();
        }
    }
}
