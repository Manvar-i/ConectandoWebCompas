using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplicationMisCompas.Model
{
    public class Alumno
    {
        public int ApplicantID { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Carrera CarreraID { get; set; }
        public string NombreCarrera { get; set; }
        public string Promedio { get; set; }
    }
}
