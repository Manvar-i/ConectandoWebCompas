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
using PokeApiNet;

namespace WebApplicationMisCompas.Pages
{
    public class PerfilEditableModel : PageModel
    {
        private readonly IConfiguration _configuration;

        int apply;
       
        public IList<Resenia> ListaResenias { get; set; }

        public Alumno alumno1 = new Alumno();

        public PerfilEditableModel(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        PokeApiClient pokeClient = new PokeApiClient();

        public async Task RandomPokemon()
        {
            Random rand = new Random();
            int n = rand.Next(1, 808);
            Console.Write(n);
            Pokemon clawFossil = await pokeClient.GetResourceAsync<Pokemon>(n);

            string PokeSprites = clawFossil.Sprites.FrontShiny;
            Console.Write(PokeSprites);
            ViewData["PokemonPic"] = PokeSprites;
        }

        private void getPerfil(string Email)
        {
           

            string connectionString = _configuration.GetConnectionString("myDb");

            MySqlConnection conexion = new MySqlConnection(connectionString);
            conexion.Open();

            MySqlCommand cmd = new MySqlCommand();
            cmd.Connection = conexion;

            Alumno alumno1 = new Alumno();

            Resenia Resenia1 = new Resenia();
            ListaResenias = new List<Resenia>();

            // Datos del alumno
            cmd.CommandText = "SELECT * FROM alumno WHERE email = " + "'" + Email + "';";
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    apply = Convert.ToInt32(reader["alumnoID"]);
                    alumno1.Name = reader["nombre"].ToString();
                    alumno1.LastName = reader["apellido"].ToString();
                    alumno1.Email = reader["email"].ToString();
                    ViewData["id"] = apply.ToString();
                    ViewData["nombre"] = alumno1.Name + ' ' + alumno1.LastName;
                    ViewData["email"] = alumno1.Email;
                }
            }

            // Carrera
            cmd.CommandText = "SELECT carrera.nombreSiglas FROM carrera INNER JOIN alumno ON alumno.carreraID = carrera.carreraID WHERE alumno.alumnoID = " + "'" + apply + "';";
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    alumno1.NombreCarrera = reader["nombreSiglas"].ToString();
                    ViewData["carrera"] = alumno1.NombreCarrera;
                }
            }

            // Rese?as
            cmd.CommandText = "SELECT * FROM resenia WHERE alumnoID = " + "'" + apply + "';";
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Resenia1 = new Resenia();
                    Resenia1.Rating = Convert.ToInt32(reader["calificacion"]);
                    Resenia1.Description = reader["descripcion"].ToString();
                    if (Resenia1.Rating != 0)
                    {
                        ListaResenias.Add(Resenia1);
                    }
                }
            }

            cmd.CommandText = "SELECT alumno.alumnoID, alumno.nombre, alumno.apellido, alumno.email, carrera.nombreSiglas, (SUM(resenia.calificacion) / (COUNT(resenia.calificacion) - 1)) AS promedio FROM alumno INNER JOIN carrera ON alumno.carreraID = carrera.carreraID INNER JOIN resenia ON resenia.alumnoID = " + "'" + apply + "' GROUP BY alumnoID;";

            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    ViewData["promed"] = reader["promedio"].ToString();
                }
            }

            conexion.Dispose();
        }
    
        public void OnPost(string Email)
        {
            getPerfil(Email);
        }

        public void OnGet(string Email)
        {
            RandomPokemon().Wait();
            ListaResenias = new List<Resenia>();
            getPerfil(Email);
        }

    }
}

