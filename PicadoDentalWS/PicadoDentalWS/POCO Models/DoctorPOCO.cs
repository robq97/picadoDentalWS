using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PicadoDentalWS.POCO_Models
{
    public class DoctorPOCO
    {
        public int PersonaID { get; set; }
        public String Nombre { get; set; }
        public String Apellidos { get; set; }
        public String Correo { get; set; }
        public String Telefono { get; set; }
        public int Cedula { get; set;}
        public String Genero { get; set; }
        
    }
}