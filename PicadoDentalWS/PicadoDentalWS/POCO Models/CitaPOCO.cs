using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PicadoDentalWS.POCO_Models
{
    public class CitaPOCO
    {
        
        public DateTime Fecha { get; set; }
        public String ClienteNombre { get; set; }
        public String ClienteApellidos{ get; set; }
        public String DoctorNombre { get; set; }
        public String DoctorApellidos{ get; set; }
        public String Detalles { get; set; }
        public int ClienteID { get; set; }
        public String DoctorTelefono { get; set; }
        public String DoctorCorreo { get; set; }
        public String ClienteTelefono { get; set; }
        public String ClienteCorreo { get; set; }
        public String Comentarios { get; set; }

    }
}